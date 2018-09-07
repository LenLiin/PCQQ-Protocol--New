using System;
using System.Text;

namespace QQ.Framework.Utils
{
    public class CRC32cs
    {
        private static ushort[] CRC16Table;

        private static uint[] CRC32Table;

        protected static ulong[] Crc32Table;

        private static void MakeCRC16Table()
        {
            if (CRC16Table != null)
            {
                return;
            }

            CRC16Table = new ushort[256];
            for (ushort num = 0; num < 256; num += 1)
            {
                var num2 = num;
                for (var i = 0; i < 8; i++)
                {
                    if (num2 % 2 == 0)
                    {
                        num2 = (ushort) (num2 >> 1);
                    }
                    else
                    {
                        num2 = (ushort) ((num2 >> 1) ^ 33800);
                    }
                }

                CRC16Table[num] = num2;
            }
        }

        private static void MakeCRC32Table()
        {
            if (CRC32Table != null)
            {
                return;
            }

            CRC32Table = new uint[256];
            for (var num = 0u; num < 256u; num += 1u)
            {
                var num2 = num;
                for (var i = 0; i < 8; i++)
                {
                    if (num2 % 2u == 0u)
                    {
                        num2 >>= 1;
                    }
                    else
                    {
                        num2 = (num2 >> 1) ^ 3988292384u;
                    }
                }

                CRC32Table[(int) (UIntPtr) num] = num2;
            }
        }

        private static ushort UpdateCRC16(byte AByte, ushort ASeed)
        {
            return (ushort) (CRC16Table[(ASeed & 255) ^ AByte] ^ (ASeed >> 8));
        }

        private static uint UpdateCRC32(byte AByte, uint ASeed)
        {
            return CRC32Table[(int) (UIntPtr) ((ASeed & 255u) ^ AByte)] ^ (ASeed >> 8);
        }

        private static ushort CRC16(byte[] ABytes)
        {
            MakeCRC16Table();
            ushort num = 65535;
            foreach (var aByte in ABytes)
            {
                num = UpdateCRC16(aByte, num);
            }

            return num;
        }

        private static ushort CRC16(string AString, Encoding AEncoding)
        {
            return CRC16(AEncoding.GetBytes(AString));
        }

        private static ushort CRC16(string AString)
        {
            return CRC16(AString, Encoding.UTF8);
        }

        public static uint CRC32(byte[] ABytes)
        {
            MakeCRC32Table();
            var num = 4294967295u;
            foreach (var aByte in ABytes)
            {
                num = UpdateCRC32(aByte, num);
            }

            return CRC32ToUint(~num);
        }

        //生成CRC32码表
        public static void GetCRC32Table()
        {
            ulong Crc;
            Crc32Table = new ulong[256];
            int i, j;
            for (i = 0; i < 256; i++)
            {
                Crc = (ulong) i;
                for (j = 8; j > 0; j--)
                {
                    if ((Crc & 1) == 1)
                    {
                        Crc = (Crc >> 1) ^ 0xEDB88320;
                    }
                    else
                    {
                        Crc >>= 1;
                    }
                }

                Crc32Table[i] = Crc;
            }
        }

        //获取字符串的CRC32校验值
        public static ulong GetCRC32Str(string sInputString)
        {
            //生成码表
            GetCRC32Table();
            var buffer = Encoding.ASCII.GetBytes(sInputString);
            ulong value = 0xffffffff;
            var len = buffer.Length;
            for (var i = 0; i < len; i++)
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
            var len = buffer.Length;
            for (var i = 0; i < len; i++)
            {
                value = (value >> 8) ^ Crc32Table[(value & 0xFF) ^ buffer[i]];
            }

            return value ^ 0xffffffff;
        }

        private static uint CRC32(string AString, Encoding AEncoding)
        {
            return CRC32(AEncoding.GetBytes(AString));
        }

        private static uint CRC32(string AString)
        {
            return CRC32(AString, Encoding.UTF8);
        }

        private static uint CRC32ToUint(uint crc32)
        {
            var text = crc32.ToString("X2");
            var text2 = "";
            for (var i = 6; i >= 0; i -= 2)
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