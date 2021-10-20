using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Lunacy.ASAR
{
    public class AsarArchive
    {
        public const int UIntSize = 4;
        public const int LongSize = 2 * UIntSize;
        public const int InfoSize = 2 * LongSize;

        public int BaseOffset { get; }

        public readonly byte[] Bytes;

        public string FilePath { get; }

        public AsarHeader Header { get; }

        public AsarArchive(string filePath)
        {
            FilePath = filePath;

            if (!File.Exists(FilePath))
                throw new FileNotFoundException("Could not find ASAR archive file.", filePath);

            try
            {
                Bytes = File.ReadAllBytes(filePath);
            }
            catch (Exception? e)
            {
                throw new AsarException("Exception thrown while reading ASAR archive.", e);
            }

            try
            {
                Header = ReadHeader(ref Bytes);
                BaseOffset = Header.Length;
            }
            catch (Exception? e)
            {
                throw new AsarException("Exception thrown while gathering header information.", e);
            }
        }

        public static AsarHeader ReadHeader(ref byte[] bytes)
        {
            byte[] headerInfo = bytes.Take(InfoSize).ToArray();

            if (headerInfo.Length < InfoSize)
                throw new AsarException("Invalid ASAR header info size.", null);

            byte[] asarFileDescriptor = headerInfo.Take(LongSize).ToArray();
            byte[] asarPayloadSize = asarFileDescriptor.Take(UIntSize).ToArray();

            int payloadSize = BitConverter.ToInt32(asarPayloadSize, 0);
            int payloadOffset = asarFileDescriptor.Length - payloadSize;

            if (payloadSize != UIntSize && payloadSize != LongSize)
                throw new AsarException("Invalid ASAR payload descriptor.", null);

            byte[] asarHeaderLength = asarFileDescriptor.Skip(payloadOffset).Take(UIntSize).ToArray();

            int headerLength = BitConverter.ToInt32(asarHeaderLength, 0);

            byte[] asarFileHeader = headerInfo.Skip(LongSize).Take(LongSize).ToArray();
            byte[] asarHeaderPayloadSize = asarFileHeader.Take(UIntSize).ToArray();

            int headerPayloadSize = BitConverter.ToInt32(asarHeaderPayloadSize, 0);
            int headerPayloadOffset = headerLength - headerPayloadSize;

            byte[] dataTableLength = asarFileHeader.Skip(headerPayloadOffset).Take(UIntSize).ToArray();
            int dataTableSize = BitConverter.ToInt32(dataTableLength, 0);

            byte[] hData = bytes.Skip(InfoSize).Take(dataTableSize).ToArray();

            if (hData.Length != dataTableSize)
                throw new AsarException("Invalid ASAR archive file size.", null);

            int asarDataOffset = asarFileDescriptor.Length + headerLength;

            JObject jObject = JObject.Parse(System.Text.Encoding.Default.GetString(hData));

            return new AsarHeader(headerInfo, asarDataOffset, hData, jObject);
        }
    }
}