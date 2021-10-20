using Newtonsoft.Json.Linq;

namespace Lunacy.ASAR
{
    public readonly struct AsarHeader
    {
        public byte[] Info { get; }

        public int Length { get; }

        public byte[] Data { get; }

        public JObject Json { get; }

        public AsarHeader(byte[] info, int length, byte[] data, JObject json)
        {
            Info = info;
            Length = length;
            Data = data;
            Json = json;
        }
    }
}