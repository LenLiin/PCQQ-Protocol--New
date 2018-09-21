using System;
using System.Security.Cryptography;

namespace QQ.Framework.Utils
{
    // Token: 0x020000C8 RID: 200
    public class XXTeaCrypter
    {
        // Token: 0x0600063A RID: 1594 RVA: 0x000265D8 File Offset: 0x000247D8
        public byte[] Encrypt(byte[] Data, byte[] Key)
        {
            byte[] result;
            if (Data.Length == 0)
            {
                result = Data;
            }
            else
            {
                result = this.ToByteArray(this.Encrypt(this.ToUInt32Array(Data, true), this.ToUInt32Array(Key, false)), false);
            }
            return result;
        }

        // Token: 0x0600063B RID: 1595 RVA: 0x00026614 File Offset: 0x00024814
        public byte[] Decrypt(byte[] Data, byte[] Key)
        {
            byte[] result;
            try
            {
                if (Data.Length == 0)
                {
                    result = Data;
                }
                else
                {
                    result = this.ToByteArray(this.Decrypt(this.ToUInt32Array(Data, false), this.ToUInt32Array(Key, false)), true);
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }
        
        public uint[] Encrypt(uint[] v, uint[] k)
        {
            int num = v.Length - 1;
            uint[] result;
            if (num < 1)
            {
                result = v;
            }
            else
            {
                if (k.Length < 4)
                {
                    uint[] array = new uint[4];
                    k.CopyTo(array, 0);
                    k = array;
                }
                uint num2 = v[num];
                uint num3 = v[0];
                uint num4 = 2654435769u;
                uint num5 = 0u;
                int num6 = 6 + 52 / (num + 1);
                while (num6-- > 0)
                {
                    num5 += num4;
                    uint num7 = num5 >> 2 & 3u;
                    int i;
                    for (i = 0; i < num; i++)
                    {
                        num3 = v[i + 1];
                        num2 = (v[i] += ((num2 >> 5 ^ num3 << 2) + (num3 >> 3 ^ num2 << 4) ^ (num5 ^ num3) + (k[(int)(checked((IntPtr)(unchecked((long)(i & 3) ^ (long)((ulong)num7)))))] ^ num2)));
                    }
                    num3 = v[0];
                    num2 = (v[num] += ((num2 >> 5 ^ num3 << 2) + (num3 >> 3 ^ num2 << 4) ^ (num5 ^ num3) + (k[(int)(checked((IntPtr)(unchecked((long)(i & 3) ^ (long)((ulong)num7)))))] ^ num2)));
                }
                result = v;
            }
            return result;
        }

        public uint[] Decrypt(uint[] v, uint[] k)
        {
            int num = v.Length - 1;
            uint[] result;
            if (num < 1)
            {
                result = v;
            }
            else
            {
                if (k.Length < 4)
                {
                    uint[] array = new uint[4];
                    k.CopyTo(array, 0);
                    k = array;
                }
                uint num2 = v[num];
                uint num3 = v[0];
                uint num4 = 2654435769u;
                int num5 = 6 + 52 / (num + 1);
                for (uint num6 = (uint)((long)num5 * 2654435769L); num6 != 0u; num6 -= num4)
                {
                    uint num7 = num6 >> 2 & 3u;
                    int i;
                    for (i = num; i > 0; i--)
                    {
                        num2 = v[i - 1];
                        num3 = (v[i] -= ((num2 >> 5 ^ num3 << 2) + (num3 >> 3 ^ num2 << 4) ^ (num6 ^ num3) + (k[(int)(checked((IntPtr)(unchecked((long)(i & 3) ^ (long)((ulong)num7)))))] ^ num2)));
                    }
                    num2 = v[num];
                    num3 = (v[0] -= ((num2 >> 5 ^ num3 << 2) + (num3 >> 3 ^ num2 << 4) ^ (num6 ^ num3) + (k[(int)(checked((IntPtr)(unchecked((long)(i & 3) ^ (long)((ulong)num7)))))] ^ num2)));
                }
                result = v;
            }
            return result;
        }

        // Token: 0x0600063E RID: 1598 RVA: 0x000268AC File Offset: 0x00024AAC
        private uint[] ToUInt32Array(byte[] Data, bool IncludeLength)
        {
            int num = ((Data.Length & 3) == 0) ? (Data.Length >> 2) : ((Data.Length >> 2) + 1);
            uint[] array;
            if (IncludeLength)
            {
                array = new uint[num + 1];
                array[num] = (uint)Data.Length;
            }
            else
            {
                array = new uint[num];
            }
            num = Data.Length;
            for (int i = 0; i < num; i++)
            {
                array[i >> 2] |= (uint)((uint)Data[i] << ((i & 3) << 3));
            }
            return array;
        }

        // Token: 0x0600063F RID: 1599 RVA: 0x00026924 File Offset: 0x00024B24
        private byte[] ToByteArray(uint[] Data, bool IncludeLength)
        {
            int num;
            if (IncludeLength)
            {
                num = (int)Data[Data.Length - 1];
            }
            else
            {
                num = Data.Length << 2;
            }
            byte[] array = new byte[num];
            for (int i = 0; i < num; i++)
            {
                array[i] = (byte)(Data[i >> 2] >> ((i & 3) << 3));
            }
            return array;
        }
    }
}

