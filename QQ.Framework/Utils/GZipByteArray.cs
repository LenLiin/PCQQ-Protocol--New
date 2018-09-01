using Ionic.Zlib;

namespace QQ.Framework.Utils
{
    public static class GZipByteArray
    {
        public static byte[] CompressBytes(string input)
        {
            var output = ZlibStream.CompressString(input);
            return output;
        }
    }
}