using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace QQ.Framework.Utils
{
    public static class QQTea
    {
        private static void code(byte[] In, int inOffset, int inPos, byte[] Out, int outOffset, int outPos, byte[] key)
        {
            if (outPos > 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    In[outOffset + outPos + i] = System.BitConverter.GetBytes(In[inOffset + inPos + i] ^ Out[outOffset + outPos + i - 8])[0];
                }
            }
            uint[] array = QQTea.FormatKey(key);
            uint num = QQTea.ConvertByteArrayToUInt(In, outOffset + outPos);
            uint num2 = QQTea.ConvertByteArrayToUInt(In, outOffset + outPos + 4);
            uint num3 = 0u;
            uint num4 = 2654435769u;
            uint num5 = 16u;
            while (num5-- > 0u)
            {
                num3 += num4;
                num += ((num2 << 4) + array[0] ^ num2 + num3 ^ (num2 >> 5) + array[1]);
                num2 += ((num << 4) + array[2] ^ num + num3 ^ (num >> 5) + array[3]);
            }
            Array.Copy(QQTea.ConvertUIntToByteArray(num), 0, Out, outOffset + outPos, 4);
            Array.Copy(QQTea.ConvertUIntToByteArray(num2), 0, Out, outOffset + outPos + 4, 4);
            if (inPos > 0)
            {
                for (int j = 0; j < 8; j++)
                {
                    Out[outOffset + outPos + j] = System.BitConverter.GetBytes(Out[outOffset + outPos + j] ^ In[inOffset + inPos + j - 8])[0];
                }
            }
        }

        private static void decode(byte[] In, int inOffset, int inPos, byte[] Out, int outOffset, int outPos, byte[] key)
        {
            if (outPos > 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    Out[outOffset + outPos + i] = System.BitConverter.GetBytes(In[inOffset + inPos + i] ^ Out[outOffset + outPos + i - 8])[0];
                }
            }
            else
            {
                Array.Copy(In, inOffset, Out, outOffset, 8);
            }
            uint[] array = QQTea.FormatKey(key);
            uint num = QQTea.ConvertByteArrayToUInt(Out, outOffset + outPos);
            uint num2 = QQTea.ConvertByteArrayToUInt(Out, outOffset + outPos + 4);
            uint num3 = 3816266640u;
            uint num4 = 2654435769u;
            uint num5 = 16u;
            while (num5-- > 0u)
            {
                num2 -= ((num << 4) + array[2] ^ num + num3 ^ (num >> 5) + array[3]);
                num -= ((num2 << 4) + array[0] ^ num2 + num3 ^ (num2 >> 5) + array[1]);
                num3 -= num4;
            }
            Array.Copy(QQTea.ConvertUIntToByteArray(num), 0, Out, outOffset + outPos, 4);
            Array.Copy(QQTea.ConvertUIntToByteArray(num2), 0, Out, outOffset + outPos + 4, 4);
        }
        public static byte[] Decrypt(byte[] In, byte[] key)
        {
            List<byte> Into = new List<byte>();
            bool tail = true;
            for (int i = In.Length - 1; i >= 0; i--)
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
                        Into.Insert(0, In[i]);
                        tail = false;
                    }
                }
                else
                {
                    Into.Insert(0, In[i]);
                }
            }

            return Decrypt(Into.ToArray(), 0, Into.Count, key);
        }
        public static byte[] Decrypt(byte[] In, int offset, int len, byte[] key)
        {
            var temp = new byte[In.Length];
            Buffer.BlockCopy(In, 0, temp, 0, In.Length);
            if (len % 8 != 0 || len < 16)
            {
                return null;
            }
            byte[] array = new byte[len];
            for (int i = 0; i < len; i += 8)
            {
                QQTea.decode(temp, offset, i, array, 0, i, key);
            }
            for (int j = 8; j < len; j++)
            {
                array[j] ^= temp[offset + j - 8];
            }
            int num = (int)(array[0] & 7);
            len = len - num - 10;
            byte[] array2 = new byte[len];
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
            Random random = new Random();
            int num = (len + 10) % 8;
            if (num != 0)
            {
                num = 8 - num;
            }
            byte[] array = new byte[len + num + 10];
            array[0] = (byte)((random.Next() & 248) | num);
            for (int i = 1; i < num + 3; i++)
            {
                array[i] = (byte)(random.Next() & 255);
            }
            Array.Copy(temp, 0, array, num + 3, len);
            for (int j = num + 3 + len; j < array.Length; j++)
            {
                array[j] = 0;
            }
            byte[] array2 = new byte[len + num + 10];
            for (int k = 0; k < array2.Length; k += 8)
            {
                QQTea.code(array, 0, k, array2, 0, k, key);
            }
            return array2;
        }

        private static uint[] FormatKey(byte[] key)
        {
            if (key.Length == 0)
            {
                throw new ArgumentException("Key must be between 1 and 16 characters in length");
            }
            byte[] array = new byte[16];
            if (key.Length < 16)
            {
                Array.Copy(key, 0, array, 0, key.Length);
                for (int i = key.Length; i < 16; i++)
                {
                    array[i] = 32;
                }
            }
            else
            {
                Array.Copy(key, 0, array, 0, 16);
            }
            uint[] array2 = new uint[4];
            int num = 0;
            for (int j = 0; j < array.Length; j += 4)
            {
                array2[num++] = QQTea.ConvertByteArrayToUInt(array, j);
            }
            return array2;
        }

        private static byte[] ConvertUIntToByteArray(uint v)
        {
            return new byte[]
            {
            (byte)(v >> 24 & 255u),
            (byte)(v >> 16 & 255u),
            (byte)(v >> 8 & 255u),
            (byte)(v & 255u)
            };
        }

        private static uint ConvertByteArrayToUInt(byte[] v, int offset)
        {
            if (offset + 4 > v.Length)
            {
                return 0u;
            }
            uint num = (uint)((uint)v[offset] << 24);
            num |= (uint)((uint)v[offset + 1] << 16);
            num |= (uint)((uint)v[offset + 2] << 8);
            return num | (uint)v[offset + 3];
        }


        /// <summary>
        /// 这是个随机因子产生器，用来填充头部的，如果为了调试，可以用一个固定值
        /// 随机因子可以使相同的明文每次加密出来的密文都不一样
        /// </summary>
        /// <returns>随机因子</returns>        
        private static int Rand()
        {
            Random random = new Random();
            return random.Next();
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static byte[] MD5(byte[] data)
        {
            MD5 MD5Instance = System.Security.Cryptography.MD5.Create();
            return MD5Instance.ComputeHash(data);
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Md5(string text)
        {
            MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            string result = "";
            foreach (byte b in buffer)
                result += b.ToString("x2");
            return result;
        }
    }
}