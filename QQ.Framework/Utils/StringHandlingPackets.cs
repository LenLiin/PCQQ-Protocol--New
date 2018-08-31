using System;
using System.IO;

namespace QQ.Framework.Utils
{
    /// <summary>
    /// 字符串方式处理报文
    /// </summary>
	public class StringHandlingPackets
	{
		private Stream stream = new MemoryStream();
        
		public StringHandlingPackets()
		{
		}

		public void Add(byte[] item)
		{
			this.stream.Write(item, 0, item.Length);
		}

		public void Add(DateTime datetime)
		{
			uint value = (uint)(datetime - DateTime.Parse("1970-1-1").ToLocalTime()).TotalSeconds;
			byte[] bytes = BitConverter.GetBytes(value);
			Array.Reverse(bytes);
			this.stream.Write(bytes, 0, bytes.Length);
		}

		public void Add(string hilString)
		{
			byte[] array = Util.HexStringToByteArray(hilString);
			this.stream.Write(array, 0, array.Length);
		}

		public void Add(uint item)
		{
			byte[] bytes = BitConverter.GetBytes(item);
			this.stream.Write(bytes, 0, bytes.Length);
		}

		public byte[] GetBytes()
		{
			this.stream.Position = 0L;
			byte[] array = new byte[this.stream.Length];
			int num = this.stream.Read(array, 0, array.Length);
			if (num > 0)
			{
				return array;
			}
			return null;
		}
	}
}
