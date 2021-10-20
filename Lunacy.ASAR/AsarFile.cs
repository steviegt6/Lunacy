using System;

namespace Lunacy.ASAR
{
    public struct AsarFile
    {
        public string Path { get; }

        public int Size { get; }

        public int Offset { get; }

        public AsarFile(string path, string fileName, int size, int offset)
        {
            Path = ParsePath(path, fileName);
            Size = size;
            Offset = offset;
        }

        // Cancer, don't know. Ask Jiiks
        public static string ParsePath(string path, string fileName)
        {
            // Console.WriteLine(path);

            path = path.Replace(".files['", System.IO.Path.DirectorySeparatorChar.ToString())
                .Replace("files['", System.IO.Path.DirectorySeparatorChar.ToString())
                .Replace("['", "")
                .Replace("']", "")[
                    ..^fileName.Length
                ].Replace(".files.", System.IO.Path.DirectorySeparatorChar.ToString())
                .Replace("files.", "") + fileName;

            return path;
        }
    }
}