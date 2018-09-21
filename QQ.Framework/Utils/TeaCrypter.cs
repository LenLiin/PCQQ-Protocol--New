using System;
using System.Security.Cryptography;

namespace QQ.Framework.Utils
{
    public class TeaCrypter
    {
        // Token: 0x06000629 RID: 1577 RVA: 0x000256F4 File Offset: 0x000238F4
        public byte[] MD5(byte[] data)
        {
            return System.Security.Cryptography.MD5.Create().ComputeHash(data);
        }

        // Token: 0x0600062A RID: 1578 RVA: 0x00025710 File Offset: 0x00023910
        private byte[] CopyMemory(byte[] arr, int arr_index, long input)
        {
            byte[] result;
            if (arr_index + 4 > arr.Length)
            {
                result = arr;
            }
            else
            {
                arr[arr_index + 3] = (byte)((input & 4278190080L) >> 24);
                arr[arr_index + 2] = (byte)((input & 16711680L) >> 16);
                arr[arr_index + 1] = (byte)((input & 65280L) >> 8);
                arr[arr_index] = (byte)(input & 255L);
                arr[arr_index] &= byte.MaxValue;
                int num = arr_index + 1;
                arr[num] &= byte.MaxValue;
                int num2 = arr_index + 2;
                arr[num2] &= byte.MaxValue;
                int num3 = arr_index + 3;
                arr[num3] &= byte.MaxValue;
                result = arr;
            }
            return result;
        }
        
        private long CopyMemory(long Out, byte[] arr, int arr_index)
        {
            long result;
            if (arr_index + 4 > arr.Length)
            {
                result = Out;
            }
            else
            {
                long num = (long)((long)arr[arr_index + 3] << 24);
                long num2 = (long)((long)arr[arr_index + 2] << 16);
                long num3 = (long)((long)arr[arr_index + 1] << 8);
                long num4 = (long)((ulong)arr[arr_index]);
                long num5 = num | num2 | num3 | num4;
                num5 &= 4294967295L;
                result = num5;
            }
            return result;
        }
        
        private long getUnsignedInt(byte[] arrayIn, int offset, int len)
        {
            long num = 0L;
            int num2;
            if (len > 8)
            {
                num2 = offset + 8;
            }
            else
            {
                num2 = offset + len;
            }
            for (int i = offset; i < num2; i++)
            {
                num <<= 8;
                num |= (long)((ulong)((ushort)(arrayIn[i] & byte.MaxValue)));
            }
            return (num & 4294967295L) | num >> 32;
        }

        // Token: 0x0600062D RID: 1581 RVA: 0x000258A8 File Offset: 0x00023AA8
        private long Rand()
        {
            return 272L;
        }

        // Token: 0x0600062E RID: 1582 RVA: 0x000258C0 File Offset: 0x00023AC0
        private byte[] Decipher(byte[] arrayIn, byte[] arrayKey, long offset)
        {
            byte[] array = new byte[24];
            byte[] array2 = new byte[8];
            byte[] result;
            if (arrayIn.Length < 8)
            {
                result = array2;
            }
            else if (arrayKey.Length < 16)
            {
                result = array2;
            }
            else
            {
                long num = 3816266640L;
                long num2 = 2654435769L;
                long num3 = this.getUnsignedInt(arrayIn, (int)offset, 4);
                long num4 = this.getUnsignedInt(arrayIn, (int)offset + 4, 4);
                long unsignedInt = this.getUnsignedInt(arrayKey, 0, 4);
                long unsignedInt2 = this.getUnsignedInt(arrayKey, 4, 4);
                long unsignedInt3 = this.getUnsignedInt(arrayKey, 8, 4);
                long unsignedInt4 = this.getUnsignedInt(arrayKey, 12, 4);
                for (int i = 1; i <= 16; i++)
                {
                    num4 -= ((num3 << 4) + unsignedInt3 ^ num3 + num ^ (num3 >> 5) + unsignedInt4);
                    num4 &= 4294967295L;
                    num3 -= ((num4 << 4) + unsignedInt ^ num4 + num ^ (num4 >> 5) + unsignedInt2);
                    num3 &= 4294967295L;
                    num -= num2;
                    num &= 4294967295L;
                }
                array = this.CopyMemory(array, 0, num3);
                array = this.CopyMemory(array, 4, num4);
                array2[0] = array[3];
                array2[1] = array[2];
                array2[2] = array[1];
                array2[3] = array[0];
                array2[4] = array[7];
                array2[5] = array[6];
                array2[6] = array[5];
                array2[7] = array[4];
                result = array2;
            }
            return result;
        }

        // Token: 0x0600062F RID: 1583 RVA: 0x00025A34 File Offset: 0x00023C34
        private byte[] Decipher(byte[] arrayIn, byte[] arrayKey)
        {
            return this.Decipher(arrayIn, arrayKey, 0L);
        }

        // Token: 0x06000630 RID: 1584 RVA: 0x00025A54 File Offset: 0x00023C54
        private byte[] Encipher(byte[] arrayIn, byte[] arrayKey, long offset)
        {
            byte[] array = new byte[8];
            byte[] array2 = new byte[24];
            byte[] result;
            if (arrayIn.Length < 8 || arrayKey.Length < 16)
            {
                result = array;
            }
            else
            {
                long num = 0L;
                long num2 = 2654435769L;
                long num3 = this.getUnsignedInt(arrayIn, (int)offset, 4);
                long num4 = this.getUnsignedInt(arrayIn, (int)offset + 4, 4);
                long unsignedInt = this.getUnsignedInt(arrayKey, 0, 4);
                long unsignedInt2 = this.getUnsignedInt(arrayKey, 4, 4);
                long unsignedInt3 = this.getUnsignedInt(arrayKey, 8, 4);
                long unsignedInt4 = this.getUnsignedInt(arrayKey, 12, 4);
                for (int i = 1; i <= 16; i++)
                {
                    num += num2;
                    num &= 4294967295L;
                    num3 += ((num4 << 4) + unsignedInt ^ num4 + num ^ (num4 >> 5) + unsignedInt2);
                    num3 &= 4294967295L;
                    num4 += ((num3 << 4) + unsignedInt3 ^ num3 + num ^ (num3 >> 5) + unsignedInt4);
                    num4 &= 4294967295L;
                }
                array2 = this.CopyMemory(array2, 0, num3);
                array2 = this.CopyMemory(array2, 4, num4);
                array[0] = array2[3];
                array[1] = array2[2];
                array[2] = array2[1];
                array[3] = array2[0];
                array[4] = array2[7];
                array[5] = array2[6];
                array[6] = array2[5];
                array[7] = array2[4];
                result = array;
            }
            return result;
        }

        // Token: 0x06000631 RID: 1585 RVA: 0x00025BB4 File Offset: 0x00023DB4
        private byte[] Encipher(byte[] arrayIn, byte[] arrayKey)
        {
            return this.Encipher(arrayIn, arrayKey, 0L);
        }

        // Token: 0x06000632 RID: 1586 RVA: 0x00025BD4 File Offset: 0x00023DD4
        private void Encrypt8Bytes()
        {
            this.Pos = 0L;
            while (this.Pos < 8L)
            {
                if (Header)
                {
                    Plain[Pos] = (byte)(Plain[Pos] ^ prePlain[Pos]);
                }
                else
                {
                    Plain[Pos] = (byte)(Plain[Pos] ^ Out[preCrypt + Pos]);
                }
                this.Pos += 1L;
            }
            byte[] array = this.Encipher(this.Plain, this.Key);
            for (int i = 0; i <= 7; i++)
            {
                this.Out[(int)(checked((IntPtr)(unchecked(this.Crypt + (long)i))))] = array[i];
            }
            this.Pos = 0L;
            while (this.Pos <= 7L)
            {
                Out[Crypt + Pos] = (byte)(Out[Crypt + Pos] ^ prePlain[Pos]);
                this.Pos += 1L;
            }
            this.Plain.CopyTo(this.prePlain, 0);
            this.preCrypt = this.Crypt;
            this.Crypt += 8L;
            this.Pos = 0L;
            this.Header = false;
        }

        // Token: 0x06000633 RID: 1587 RVA: 0x00025D88 File Offset: 0x00023F88
        private bool Decrypt8Bytes(byte[] arrayIn, long offset)
        {
            this.Pos = 0L;
            while (this.Pos <= 7L)
            {
                if (this.contextStart + Pos > arrayIn.Length - 1)
                {
                    return true;
                }
                prePlain[Pos] = (byte)(prePlain[Pos] ^ arrayIn[offset + Crypt + Pos]);
                this.Pos += 1L;
            }
            try
            {
                this.prePlain = this.Decipher(this.prePlain, this.Key);
            }
            catch
            {
                return false;
            }
            this.contextStart += 8L;
            this.Crypt += 8L;
            this.Pos = 0L;
            return true;
        }

        // Token: 0x06000634 RID: 1588 RVA: 0x00025E90 File Offset: 0x00024090
        private bool Decrypt8Bytes(byte[] arrayIn)
        {
            return this.Decrypt8Bytes(arrayIn, 0L);
        }

        // Token: 0x06000635 RID: 1589 RVA: 0x00025EB0 File Offset: 0x000240B0
        public byte[] Encrypt(byte[] arrayIn, byte[] arrayKey, long offset)
        {
            this.Plain = new byte[8];
            this.prePlain = new byte[8];
            this.Pos = 1L;
            this.padding = 0L;
            this.preCrypt = 0L;
            this.Crypt = 0L;
            this.Key = arrayKey;
            this.Header = true;
            this.Pos = 2L;
            this.Pos = (long)((arrayIn.Length + 10) % 8);
            if (this.Pos != 0L)
            {
                this.Pos = 8L - this.Pos;
            }
            this.Out = new byte[(long)arrayIn.Length + this.Pos + 10L];
            this.Plain[0] = (byte)((this.Rand() & 248L) | this.Pos);
            int num = 1;
            while ((long)num <= this.Pos)
            {
                this.Plain[num] = (byte)(this.Rand() & 255L);
                num++;
            }
            this.Pos += 1L;
            this.padding = 1L;
            while (this.padding < 3L)
            {
                if (this.Pos < 8L)
                {
                    this.Plain[(int)(checked((IntPtr)this.Pos))] = (byte)(this.Rand() & 255L);
                    this.padding += 1L;
                    this.Pos += 1L;
                }
                else if (this.Pos == 8L)
                {
                    this.Encrypt8Bytes();
                }
            }
            int num2 = (int)offset;
            long num3 = (long)arrayIn.Length;
            while (num3 > 0L)
            {
                if (this.Pos < 8L)
                {
                    this.Plain[(int)(checked((IntPtr)this.Pos))] = arrayIn[num2];
                    num2++;
                    this.Pos += 1L;
                    num3 -= 1L;
                }
                else if (this.Pos == 8L)
                {
                    this.Encrypt8Bytes();
                }
            }
            this.padding = 1L;
            while (this.padding < 9L)
            {
                if (this.Pos < 8L)
                {
                    this.Plain[(int)(checked((IntPtr)this.Pos))] = 0;
                    this.Pos += 1L;
                    this.padding += 1L;
                }
                else if (this.Pos == 8L)
                {
                    this.Encrypt8Bytes();
                }
            }
            return this.Out;
        }

        // Token: 0x06000636 RID: 1590 RVA: 0x000261C0 File Offset: 0x000243C0
        public byte[] Encrypt(byte[] arrayIn, byte[] arrayKey)
        {
            byte[] array = null;
            int num = 0;
            for (; ; )
            {
                if (array == null)
                {
                    goto IL_22;
                }
                bool flag = false;
                IL_08:
                if (flag)
                {
                    try
                    {
                        array = this.Encrypt(arrayIn, arrayKey, 0L);
                        goto IL_28;
                    }
                    catch
                    {
                        goto IL_28;
                    }
                    goto IL_22;
                    IL_28:
                    num++;
                    continue;
                }
                break;
                IL_22:
                flag = (num < 2);
                goto IL_08;
            }
            return array;
        }

        // Token: 0x06000637 RID: 1591 RVA: 0x00026210 File Offset: 0x00024410
        public byte[] Decrypt(byte[] inData, byte[] key)
        {
            byte[] result = new byte[0];
            try
            {
                result = this.Decrypt(inData, key, 0L);
            }
            catch
            {
            }
            return result;
        }

        // Token: 0x06000638 RID: 1592 RVA: 0x00026250 File Offset: 0x00024450
        public byte[] Decrypt(byte[] arrayIn, byte[] arrayKey, long offset)
        {
            byte[] array = new byte[0];
            byte[] result;
            if (arrayIn.Length < 16 || arrayIn.Length % 8 != 0)
            {
                result = array;
            }
            else
            {
                byte[] array2 = new byte[offset + 8L];
                arrayKey.CopyTo(this.Key, 0);
                this.preCrypt = 0L;
                this.Crypt = 0L;
                this.prePlain = this.Decipher(arrayIn, arrayKey, offset);
                this.Pos = (long)(this.prePlain[0] & 7);
                long num = (long)arrayIn.Length - this.Pos - 10L;
                if (num <= 0L)
                {
                    result = array;
                }
                else
                {
                    this.Out = new byte[num];
                    this.preCrypt = 0L;
                    this.Crypt = 8L;
                    this.contextStart = 8L;
                    this.Pos += 1L;
                    this.padding = 1L;
                    while (this.padding < 3L)
                    {
                        if (this.Pos < 8L)
                        {
                            this.Pos += 1L;
                            this.padding += 1L;
                        }
                        else if (this.Pos == 8L)
                        {
                            for (int i = 0; i < array2.Length; i++)
                            {
                                array2[i] = arrayIn[i];
                            }
                            if (!this.Decrypt8Bytes(arrayIn, offset))
                            {
                                return array;
                            }
                        }
                    }
                    long num2 = 0L;
                    while (num != 0L)
                    {
                        if (this.Pos < 8L)
                        {
                            checked
                            {
                                Out[num2] = (byte)(array2[offset + preCrypt + Pos] ^ prePlain[Pos]);
                            }
                            num2 += 1L;
                            num -= 1L;
                            this.Pos += 1L;
                        }
                        else if (this.Pos == 8L)
                        {
                            array2 = arrayIn;
                            this.preCrypt = this.Crypt - 8L;
                            if (!this.Decrypt8Bytes(arrayIn, offset))
                            {
                                return array;
                            }
                        }
                    }
                    this.padding = 1L;
                    while (this.padding <= 7L)
                    {
                        if (this.Pos < 8L)
                        {
                            if (checked(array2[(int)((IntPtr)(unchecked(offset + this.preCrypt + this.Pos)))] ^ this.prePlain[(int)((IntPtr)this.Pos)]) != 0)
                            {
                                return array;
                            }
                            this.Pos += 1L;
                        }
                        else if (this.Pos == 8L)
                        {
                            for (int i = 0; i < array2.Length; i++)
                            {
                                array2[i] = arrayIn[i];
                            }
                            this.preCrypt = this.Crypt;
                            if (!this.Decrypt8Bytes(arrayIn, offset))
                            {
                                return array;
                            }
                        }
                        this.padding += 1L;
                    }
                    result = this.Out;
                }
            }
            return result;
        }

        // Token: 0x04000334 RID: 820
        private long contextStart;

        // Token: 0x04000335 RID: 821
        private long Crypt;

        // Token: 0x04000336 RID: 822
        private long preCrypt;

        // Token: 0x04000337 RID: 823
        private bool Header;

        // Token: 0x04000338 RID: 824
        private byte[] Key = new byte[16];

        // Token: 0x04000339 RID: 825
        private byte[] Out;

        // Token: 0x0400033A RID: 826
        private long padding;

        // Token: 0x0400033B RID: 827
        private byte[] Plain;

        // Token: 0x0400033C RID: 828
        private long Pos;

        // Token: 0x0400033D RID: 829
        private byte[] prePlain;
    }
}

