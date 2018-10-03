using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace QQ.Framework.Utils
{
    public static class QQTea
    {
        private static void Code(byte[] In, int inOffset, int inPos, byte[] Out, int outOffset, int outPos, byte[] key)
        {
            if (outPos > 0)
            {
                for (var i = 0; i < 8; i++)
                {
                    In[outOffset + outPos + i] =
                        BitConverter.GetBytes(In[inOffset + inPos + i] ^ Out[outOffset + outPos + i - 8])[0];
                }
            }

            var array = FormatKey(key);
            var num = ConvertByteArrayToUInt(In, outOffset + outPos);
            var num2 = ConvertByteArrayToUInt(In, outOffset + outPos + 4);
            var num3 = 0u;
            var num4 = 2654435769u;
            var num5 = 16u;
            while (num5-- > 0u)
            {
                num3 += num4;
                num += ((num2 << 4) + array[0]) ^ (num2 + num3) ^ ((num2 >> 5) + array[1]);
                num2 += ((num << 4) + array[2]) ^ (num + num3) ^ ((num >> 5) + array[3]);
            }

            Array.Copy(ConvertUIntToByteArray(num), 0, Out, outOffset + outPos, 4);
            Array.Copy(ConvertUIntToByteArray(num2), 0, Out, outOffset + outPos + 4, 4);
            if (inPos > 0)
            {
                for (var j = 0; j < 8; j++)
                {
                    Out[outOffset + outPos + j] =
                        BitConverter.GetBytes(Out[outOffset + outPos + j] ^ In[inOffset + inPos + j - 8])[0];
                }
            }
        }

        private static void Decode(byte[] In, int inOffset, int inPos, byte[] Out, int outOffset, int outPos,
            byte[] key)
        {
            if (outPos > 0)
            {
                for (var i = 0; i < 8; i++)
                {
                    Out[outOffset + outPos + i] =
                        BitConverter.GetBytes(In[inOffset + inPos + i] ^ Out[outOffset + outPos + i - 8])[0];
                }
            }
            else
            {
                Array.Copy(In, inOffset, Out, outOffset, 8);
            }

            var array = FormatKey(key);
            var num = ConvertByteArrayToUInt(Out, outOffset + outPos);
            var num2 = ConvertByteArrayToUInt(Out, outOffset + outPos + 4);
            var num3 = 3816266640u;
            var num4 = 2654435769u;
            var num5 = 16u;
            while (num5-- > 0u)
            {
                num2 -= ((num << 4) + array[2]) ^ (num + num3) ^ ((num >> 5) + array[3]);
                num -= ((num2 << 4) + array[0]) ^ (num2 + num3) ^ ((num2 >> 5) + array[1]);
                num3 -= num4;
            }

            Array.Copy(ConvertUIntToByteArray(num), 0, Out, outOffset + outPos, 4);
            Array.Copy(ConvertUIntToByteArray(num2), 0, Out, outOffset + outPos + 4, 4);
        }

        public static byte[] Decrypt(byte[] In, byte[] key)
        {
            var into = new List<byte>();
            var tail = true;
            for (var i = In.Length - 1; i >= 0; i--)
            {
                if (tail)
                {
                    if (In[i] == 0x03)
                    {
                        tail = false;
                    }
                    else if (In[i] == 0x00)
                    {
                    }
                    else
                    {
                        into.Insert(0, In[i]);
                        tail = false;
                    }
                }
                else
                {
                    into.Insert(0, In[i]);
                }
            }

            return Decrypt(into.ToArray(), 0, into.Count, key);
        }

        public static byte[] Decrypt(byte[] In, int offset, int len, byte[] key)
        {
            var temp = new byte[In.Length];
            Buffer.BlockCopy(In, 0, temp, 0, In.Length);
            if (len % 8 != 0 || len < 16)
            {
                return null;
            }

            var array = new byte[len];
            for (var i = 0; i < len; i += 8)
            {
                Decode(temp, offset, i, array, 0, i, key);
            }

            for (var j = 8; j < len; j++)
            {
                array[j] ^= temp[offset + j - 8];
            }

            var num = array[0] & 7;
            len = len - num - 10;
            var array2 = new byte[len];
            Array.Copy(array, num + 3, array2, 0, len);
            return array2;
        }

        public static byte[] Encrypt(byte[] In, byte[] key)
        {
            return Encrypt(In, 0, In.Length, key);
        }

        public static byte[] Encrypt(byte[] In, int offset, int len, byte[] key)
        {
            var temp = new byte[In.Length];
            Buffer.BlockCopy(In, 0, temp, 0, In.Length);
            var random = new Random();
            var num = (len + 10) % 8;
            if (num != 0)
            {
                num = 8 - num;
            }

            var array = new byte[len + num + 10];
            array[0] = (byte) ((random.Next() & 248) | num);
            for (var i = 1; i < num + 3; i++)
            {
                array[i] = (byte) (random.Next() & 255);
            }

            Array.Copy(temp, 0, array, num + 3, len);
            for (var j = num + 3 + len; j < array.Length; j++)
            {
                array[j] = 0;
            }

            var array2 = new byte[len + num + 10];
            for (var k = 0; k < array2.Length; k += 8)
            {
                Code(array, 0, k, array2, 0, k, key);
            }

            return array2;
        }

        private static uint[] FormatKey(byte[] key)
        {
            if (key.Length == 0)
            {
                throw new ArgumentException("Key must be between 1 and 16 characters in length");
            }

            var array = new byte[16];
            if (key.Length < 16)
            {
                Array.Copy(key, 0, array, 0, key.Length);
                for (var i = key.Length; i < 16; i++)
                {
                    array[i] = 32;
                }
            }
            else
            {
                Array.Copy(key, 0, array, 0, 16);
            }

            var array2 = new uint[4];
            var num = 0;
            for (var j = 0; j < array.Length; j += 4)
            {
                array2[num++] = ConvertByteArrayToUInt(array, j);
            }

            return array2;
        }

        private static byte[] ConvertUIntToByteArray(uint v)
        {
            return new[]
            {
                (byte) ((v >> 24) & 255u),
                (byte) ((v >> 16) & 255u),
                (byte) ((v >> 8) & 255u),
                (byte) (v & 255u)
            };
        }

        private static uint ConvertByteArrayToUInt(byte[] v, int offset)
        {
            if (offset + 4 > v.Length)
            {
                return 0u;
            }

            var num = (uint) v[offset] << 24;
            num |= (uint) v[offset + 1] << 16;
            num |= (uint) v[offset + 2] << 8;
            return num | v[offset + 3];
        }


        /// <summary>
        ///     这是个随机因子产生器，用来填充头部的，如果为了调试，可以用一个固定值
        ///     随机因子可以使相同的明文每次加密出来的密文都不一样
        /// </summary>
        /// <returns>随机因子</returns>
        private static int Rand()
        {
            var random = new Random();
            return random.Next();
        }

        /// <summary>
        ///     MD5加密
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static byte[] MD5(byte[] data)
        {
            var md5Instance = System.Security.Cryptography.MD5.Create();
            return md5Instance.ComputeHash(data);
        }

        /// <summary>
        ///     MD5加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Md5(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            var result = "";
            foreach (var b in buffer)
            {
                result += b.ToString("x2");
            }

            return result;
        }
    }
}