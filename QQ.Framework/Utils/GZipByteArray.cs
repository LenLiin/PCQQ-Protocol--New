using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Utils
{
    public static class GZipByteArray
    {

        public static byte[] CompressBytes(string input)
        {
            byte[] output = ZlibStream.CompressString(input);
            return output;
        }
    }
}
