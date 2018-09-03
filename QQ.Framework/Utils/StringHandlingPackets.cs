using System;
using System.IO;

namespace QQ.Framework.Utils
{
    /// <summary>
    ///     字符串方式处理报文
    /// </summary>
    public class StringHandlingPackets
    {
        private readonly Stream stream = new MemoryStream();

        public void Add(byte[] item)
        {
            stream.Write(item, 0, item.Length);
        }

        public void Add(DateTime datetime)
        {
            var value = (uint) (datetime - DateTime.Parse("1970-1-1").ToLocalTime()).TotalSeconds;
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            stream.Write(bytes, 0, bytes.Length);
        }

        public void Add(string hilString)
        {
            var array = Util.HexStringToByteArray(hilString);
            stream.Write(array, 0, array.Length);
        }

        public void Add(uint item)
        {
            var bytes = BitConverter.GetBytes(item);
            stream.Write(bytes, 0, bytes.Length);
        }

        public byte[] GetBytes()
        {
            stream.Position = 0L;
            var array = new byte[stream.Length];
            var num = stream.Read(array, 0, array.Length);
            if (num > 0)
            {
                return array;
            }

            return null;
        }
    }
}