using Ionic.Zlib;
using System.Text;

namespace QQ.Framework.Utils
{
    public static class GZipByteArray
    {
        public static byte[] CompressBytes(string input)
        {
            var output = ZlibStream.CompressString(input);
            return output;
        }
        public static string DecompressString(byte[] input)
        {
            var output = Encoding.UTF8.GetString(ZlibStream.UncompressBuffer(input));
            return output;
        }
    }
}