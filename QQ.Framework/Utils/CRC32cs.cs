using System;
using System.Text;

namespace QQ.Framework.Utils
{
	public class CRC32cs
	{
		private static ushort[] CRC16Table;

		private static uint[] CRC32Table;

		private static void MakeCRC16Table()
		{
			if (CRC32cs.CRC16Table != null)
			{
				return;
			}
			CRC32cs.CRC16Table = new ushort[256];
			for (ushort num = 0; num < 256; num += 1)
			{
				ushort num2 = num;
				for (int i = 0; i < 8; i++)
				{
					if (num2 % 2 == 0)
					{
						num2 = (ushort)(num2 >> 1);
					}
					else
					{
						num2 = (ushort)(num2 >> 1 ^ 33800);
					}
				}
				CRC32cs.CRC16Table[(int)num] = num2;
			}
		}

		private static void MakeCRC32Table()
		{
			if (CRC32cs.CRC32Table != null)
			{
				return;
			}
			CRC32cs.CRC32Table = new uint[256];
			for (uint num = 0u; num < 256u; num += 1u)
			{
				uint num2 = num;
				for (int i = 0; i < 8; i++)
				{
					if (num2 % 2u == 0u)
					{
						num2 >>= 1;
					}
					else
					{
						num2 = (num2 >> 1 ^ 3988292384u);
					}
				}
				CRC32cs.CRC32Table[(int)((UIntPtr)num)] = num2;
			}
		}

		private static ushort UpdateCRC16(byte AByte, ushort ASeed)
		{
			return (ushort)((int)CRC32cs.CRC16Table[(int)((ASeed & 255) ^ (ushort)AByte)] ^ ASeed >> 8);
		}

		private static uint UpdateCRC32(byte AByte, uint ASeed)
		{
			return CRC32cs.CRC32Table[(int)((UIntPtr)((ASeed & 255u) ^ (uint)AByte))] ^ ASeed >> 8;
		}

		private static ushort CRC16(byte[] ABytes)
		{
			CRC32cs.MakeCRC16Table();
			ushort num = 65535;
			for (int i = 0; i < ABytes.Length; i++)
			{
				byte aByte = ABytes[i];
				num = CRC32cs.UpdateCRC16(aByte, num);
			}
			return num;
		}

		private static ushort CRC16(string AString, Encoding AEncoding)
		{
			return CRC32cs.CRC16(AEncoding.GetBytes(AString));
		}

		private static ushort CRC16(string AString)
		{
			return CRC32cs.CRC16(AString, Encoding.UTF8);
		}

		public static uint CRC32(byte[] ABytes)
		{
			CRC32cs.MakeCRC32Table();
			uint num = 4294967295u;
			for (int i = 0; i < ABytes.Length; i++)
			{
				byte aByte = ABytes[i];
				num = CRC32cs.UpdateCRC32(aByte, num);
			}
			return CRC32cs.CRC32ToUint(~num);
		}

        protected static ulong[] Crc32Table;
        //生成CRC32码表
        public static void GetCRC32Table()
        {
            ulong Crc;
            Crc32Table = new ulong[256];
            int i, j;
            for (i = 0; i < 256; i++)
            {
                Crc = (ulong)i;
                for (j = 8; j > 0; j--)
                {
                    if ((Crc & 1) == 1)
                        Crc = (Crc >> 1) ^ 0xEDB88320;
                    else
                        Crc >>= 1;
                }
                Crc32Table[i] = Crc;
            }
        }

        //获取字符串的CRC32校验值
        public static ulong GetCRC32Str(string sInputString)
        {
            //生成码表
            GetCRC32Table();
            byte[] buffer = System.Text.ASCIIEncoding.ASCII.GetBytes(sInputString);
            ulong value = 0xffffffff;
            int len = buffer.Length;
            for (int i = 0; i < len; i++)
            {
                value = (value >> 8) ^ Crc32Table[(value & 0xFF) ^ buffer[i]];
            }
            return value ^ 0xffffffff;
        }
        public static ulong GetCRC32(byte[] buffer)
        {
            //生成码表
            GetCRC32Table();
            ulong value = 0xffffffff;
            int len = buffer.Length;
            for (int i = 0; i < len; i++)
            {
                value = (value >> 8) ^ Crc32Table[(value & 0xFF) ^ buffer[i]];
            }
            return value ^ 0xffffffff;
        }

        private static uint CRC32(string AString, Encoding AEncoding)
		{
			return CRC32cs.CRC32(AEncoding.GetBytes(AString));
		}

		private static uint CRC32(string AString)
		{
			return CRC32cs.CRC32(AString, Encoding.UTF8);
		}

		private static uint CRC32ToUint(uint crc32)
		{
			string text = crc32.ToString("X2");
			string text2 = "";
			for (int i = 6; i >= 0; i -= 2)
			{
				text2 = text2 + text.Substring(i, 2) + " ";
			}
			uint result;
			try
			{
				result = Convert.ToUInt32(text2.Trim(), 16);
			}
			catch
			{
				result = 0u;
			}
			return result;
		}
	}
}
