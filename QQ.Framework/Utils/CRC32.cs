using System;
using System.Text;

namespace QQ.Framework.Utils
{
    public class CRC32
    {
        private static ushort[] _crc16Table;

        private static uint[] _crc32Table;

        protected static ulong[] _crc32Table2;

        private static void MakeCRC16Table()
        {
            if (_crc16Table != null)
            {
                return;
            }

            _crc16Table = new ushort[256];
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

                _crc16Table[num] = num2;
            }
        }

        private static void MakeCRC32Table()
        {
            if (_crc32Table != null)
            {
                return;
            }

            _crc32Table = new uint[256];
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

                _crc32Table[(int) (UIntPtr) num] = num2;
            }
        }

        private static ushort UpdateCRC16(byte aByte, ushort aSeed)
        {
            return (ushort) (_crc16Table[(aSeed & 255) ^ aByte] ^ (aSeed >> 8));
        }

        private static uint UpdateCRC32(byte aByte, uint aSeed)
        {
            return _crc32Table[(int) (UIntPtr) ((aSeed & 255u) ^ aByte)] ^ (aSeed >> 8);
        }

        private static ushort CRC16(byte[] aBytes)
        {
            MakeCRC16Table();
            ushort num = 65535;
            foreach (var aByte in aBytes)
            {
                num = UpdateCRC16(aByte, num);
            }

            return num;
        }

        private static ushort CRC16(string aString, Encoding aEncoding)
        {
            return CRC16(aEncoding.GetBytes(aString));
        }

        private static ushort CRC16(string aString)
        {
            return CRC16(aString, Encoding.UTF8);
        }

        public static uint CRC32Imp(byte[] aBytes)
        {
            MakeCRC32Table();
            var num = 4294967295u;
            foreach (var aByte in aBytes)
            {
                num = UpdateCRC32(aByte, num);
            }

            return CRC32ToUint(~num);
        }

        public static uint CRC32Reverse(byte[] aBytes)
        {
            MakeCRC32Table();
            var num = 4294967295u;
            foreach (var aByte in aBytes)
            {
                num = UpdateCRC32(aByte, num);
            }

            return CRC32ToUintReverse(~num);
        }

        //生成CRC32码表
        public static void GetCRC32Table()
        {
            _crc32Table2 = new ulong[256];
            int i;
            for (i = 0; i < 256; i++)
            {
                var crc = (ulong) i;
                int j;
                for (j = 8; j > 0; j--)
                {
                    if ((crc & 1) == 1)
                    {
                        crc = (crc >> 1) ^ 0xEDB88320;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }

                _crc32Table2[i] = crc;
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
                value = (value >> 8) ^ _crc32Table2[(value & 0xFF) ^ buffer[i]];
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
                value = (value >> 8) ^ _crc32Table2[(value & 0xFF) ^ buffer[i]];
            }

            return value ^ 0xffffffff;
        }

        private static uint CRC32Imp(string aString, Encoding aEncoding)
        {
            return CRC32Imp(aEncoding.GetBytes(aString));
        }

        private static uint CRC32Imp(string aString)
        {
            return CRC32Imp(aString, Encoding.UTF8);
        }

        private static uint CRC32ToUint(uint crc32)
        {
            var text = crc32.ToString("X2");
            if (text.Length == 7)
            {
                text = "0" + text;
            }

            var text2 = "";
            for (var i = 6; i >= 0; i -= 2)
            {
                text2 = text2 + text.Substring(i, 2) + " ";
            }

            uint result;
            try
            {
                result = Convert.ToUInt32(text2.Replace(" ", ""), 16);
            }
            catch
            {
                result = 0u;
            }

            return result;
        }

        private static uint CRC32ToUintReverse(uint crc32)
        {
            var text = crc32.ToString("X2");
            var text2 = "";
            for (var i = 6; i >= 0; i -= 2)
            {
                text2 = text.Substring(i, 2) + " " + text2;
            }

            uint result;
            try
            {
                result = Convert.ToUInt32(text2.Replace(" ", ""), 16);
            }
            catch
            {
                result = 0u;
            }

            return result;
        }
    }
}