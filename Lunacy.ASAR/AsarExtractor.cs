using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Lunacy.ASAR
{
    public class AsarExtractor
    {
        public event EventHandler<AsarExtractEvent>? FileExtracted;
        public event EventHandler<AsarExtractFinishedEvent>? Finished;

        public List<AsarFile>? FilesToExtract;
        public bool EmptyDirectory;

        public async Task Extract(AsarArchive archive, string path, string destination)
        {
            try
            {
                string[] pathArr = path.Split('/');
                JToken token = pathArr.Aggregate<string, JToken>(archive.Header.Json, (current, t) =>
                    current["files"]?[t] ?? ""
                );
                int size = token.Value<int>("size");
                int offset = archive.BaseOffset + token.Value<int>("offset");
                byte[] fileBytes = archive.Bytes.Skip(offset).Take(size).ToArray();

                string? dirPath = Path.GetDirectoryName(destination);

                Directory.CreateDirectory(dirPath ?? "");

                await File.WriteAllBytesAsync(destination, fileBytes);
            }
            catch (Exception e)
            {
                await Task.FromException(e);
            }
        }

        public async Task<bool> ExtractAll(AsarArchive archive, string destination, bool emptyDir = false)
        {
            FilesToExtract = new List<AsarFile>();
            EmptyDirectory = emptyDir;

            JObject jObject = archive.Header.Json;

            if (jObject.HasValues)
                TokenIterator(jObject.First);

            byte[] bytes = archive.Bytes;
            int progress = 0;

            foreach (AsarFile asarFile in FilesToExtract)
            {
                progress++;
                int size = asarFile.Size;
                int offset = archive.BaseOffset + asarFile.Offset;

                if (size > -1)
                {
                    byte[] fileBytes = new byte[size];

                    Buffer.BlockCopy(bytes, offset, fileBytes, 0, size);
                    string filePath = $"{destination}{asarFile.Path}";
                    string? dirPath = Path.GetDirectoryName(filePath);

                    Directory.CreateDirectory(dirPath ?? "");

                    await File.WriteAllBytesAsync(filePath, fileBytes);

                    FileExtracted?.Invoke(this, new AsarExtractEvent(asarFile, progress, FilesToExtract.Count));
                }
                else
                {
                    if (emptyDir)
                        Directory.CreateDirectory($"{destination}{asarFile.Path}");
                }
            }

            Finished?.Invoke(this, new AsarExtractFinishedEvent(true));

            return true;
        }

        public void TokenIterator(JToken? token)
        {
            if (token is not JProperty property)
                return;

            foreach (JToken jToken in property.Value.Children())
            {
                JProperty? prop = jToken as JProperty;
                int size = -1;
                int offset = -1;
                AsarFile aFile;

                if (prop is null)
                    continue;

                foreach (JToken jToken1 in prop.Value.Children())
                {
                    JProperty? nextProp = jToken1 as JProperty;

                    if (nextProp is null)
                        continue;

                    switch (nextProp.Name)
                    {
                        case "files":
                        {
                            if (EmptyDirectory)
                            {
                                // Console.WriteLine($"PROP PATH: {prop.Path}");
                                aFile = new AsarFile(prop.Path, "", size, offset);
                                FilesToExtract?.Add(aFile);
                            }

                            TokenIterator(nextProp);
                            break;
                        }

                        case "size":
                            size = int.Parse(nextProp.Value.ToString());
                            break;

                        case "offset":
                            offset = int.Parse(nextProp.Value.ToString());
                            break;
                    }
                }

                if (size <= -1 || offset <= -1)
                    continue;

                aFile = new AsarFile(prop.Path, prop.Name, size, offset);
                FilesToExtract?.Add(aFile);
            }
        }
    }
}