using System.Text;
using Ionic.Zlib;

namespace QQ.Framework.Utils
{
    public static class GZipByteArray
    {
        public static byte[] CompressBytes(string input)
        {
            return ZlibStream.CompressString(input);
        }

        public static string DecompressString(byte[] input)
        {
            return Encoding.UTF8.GetString(ZlibStream.UncompressBuffer(input));
        }
    }
}