namespace QQ.Framework.Utils
{
    public class TeaCrypter
    {
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

        // Token: 0x06000629 RID: 1577 RVA: 0x000256F4 File Offset: 0x000238F4
        public byte[] MD5(byte[] data)
        {
            return System.Security.Cryptography.MD5.Create().ComputeHash(data);
        }

        // Token: 0x0600062A RID: 1578 RVA: 0x00025710 File Offset: 0x00023910
        private byte[] CopyMemory(byte[] arr, int arr_index, long input)
        {
            if (arr_index + 4 > arr.Length)
            {
                return arr;
            }

            arr[arr_index + 3] = (byte) ((input & 4278190080u) >> 24);
            arr[arr_index + 2] = (byte) ((input & 0xFF0000) >> 16);
            arr[arr_index + 1] = (byte) ((input & 0xFF00) >> 8);
            arr[arr_index] = (byte) (input & 0xFF);
            arr[arr_index] &= byte.MaxValue;
            arr[arr_index + 1] &= byte.MaxValue;
            arr[arr_index + 2] &= byte.MaxValue;
            arr[arr_index + 3] &= byte.MaxValue;
            return arr;
        }

        private long CopyMemory(long Out, byte[] arr, int arr_index)
        {
            if (arr_index + 4 > arr.Length)
            {
                return Out;
            }

            long num = arr[arr_index + 3] << 24;
            long num2 = arr[arr_index + 2] << 16;
            long num3 = arr[arr_index + 1] << 8;
            long num4 = arr[arr_index];
            var num5 = num | num2 | num3 | num4;
            return num5 & uint.MaxValue;
        }

        private long getUnsignedInt(byte[] arrayIn, int offset, int len)
        {
            var num = 0L;
            var num2 = len <= 8 ? offset + len : offset + 8;
            for (var i = offset; i < num2; i++)
            {
                num <<= 8;
                num |= (ushort) (arrayIn[i] & 0xFF);
            }

            return (num & uint.MaxValue) | (num >> 32);
        }

        // Token: 0x0600062D RID: 1581 RVA: 0x000258A8 File Offset: 0x00023AA8
        private long Rand()
        {
            return 272L;
        }

        // Token: 0x0600062E RID: 1582 RVA: 0x000258C0 File Offset: 0x00023AC0
        private byte[] Decipher(byte[] arrayIn, byte[] arrayKey, long offset = 0L)
        {
            var arr = new byte[24];
            var array = new byte[8];
            if (arrayIn.Length < 8)
            {
                return array;
            }

            if (arrayKey.Length < 16)
            {
                return array;
            }

            var num = 3816266640L;
            num &= uint.MaxValue;
            var num2 = 2654435769L;
            num2 &= uint.MaxValue;
            var num3 = getUnsignedInt(arrayIn, (int) offset, 4);
            var num4 = getUnsignedInt(arrayIn, (int) offset + 4, 4);
            var unsignedInt = getUnsignedInt(arrayKey, 0, 4);
            var unsignedInt2 = getUnsignedInt(arrayKey, 4, 4);
            var unsignedInt3 = getUnsignedInt(arrayKey, 8, 4);
            var unsignedInt4 = getUnsignedInt(arrayKey, 12, 4);
            for (var i = 1; i <= 16; i++)
            {
                num4 -= ((num3 << 4) + unsignedInt3) ^ (num3 + num) ^ ((num3 >> 5) + unsignedInt4);
                num4 &= uint.MaxValue;
                num3 -= ((num4 << 4) + unsignedInt) ^ (num4 + num) ^ ((num4 >> 5) + unsignedInt2);
                num3 &= uint.MaxValue;
                num -= num2;
                num &= uint.MaxValue;
            }

            arr = CopyMemory(arr, 0, num3);
            arr = CopyMemory(arr, 4, num4);
            array[0] = arr[3];
            array[1] = arr[2];
            array[2] = arr[1];
            array[3] = arr[0];
            array[4] = arr[7];
            array[5] = arr[6];
            array[6] = arr[5];
            array[7] = arr[4];
            return array;
        }

        // Token: 0x06000630 RID: 1584 RVA: 0x00025A54 File Offset: 0x00023C54
        private byte[] Encipher(byte[] arrayIn, byte[] arrayKey, long offset = 0L)
        {
            var array = new byte[8];
            var arr = new byte[24];
            if (arrayIn.Length < 8 || arrayKey.Length < 16)
            {
                return array;
            }

            var num = 0L;
            var num2 = 2654435769L;
            num2 &= uint.MaxValue;
            var num3 = getUnsignedInt(arrayIn, (int) offset, 4);
            var num4 = getUnsignedInt(arrayIn, (int) offset + 4, 4);
            var unsignedInt = getUnsignedInt(arrayKey, 0, 4);
            var unsignedInt2 = getUnsignedInt(arrayKey, 4, 4);
            var unsignedInt3 = getUnsignedInt(arrayKey, 8, 4);
            var unsignedInt4 = getUnsignedInt(arrayKey, 12, 4);
            for (var i = 1; i <= 16; i++)
            {
                num += num2;
                num &= uint.MaxValue;
                num3 += ((num4 << 4) + unsignedInt) ^ (num4 + num) ^ ((num4 >> 5) + unsignedInt2);
                num3 &= uint.MaxValue;
                num4 += ((num3 << 4) + unsignedInt3) ^ (num3 + num) ^ ((num3 >> 5) + unsignedInt4);
                num4 &= uint.MaxValue;
            }

            arr = CopyMemory(arr, 0, num3);
            arr = CopyMemory(arr, 4, num4);
            array[0] = arr[3];
            array[1] = arr[2];
            array[2] = arr[1];
            array[3] = arr[0];
            array[4] = arr[7];
            array[5] = arr[6];
            array[6] = arr[5];
            array[7] = arr[4];
            return array;
        }

        // Token: 0x06000632 RID: 1586 RVA: 0x00025BD4 File Offset: 0x00023DD4
        private void Encrypt8Bytes()
        {
            for (Pos = 0L; Pos < 8; Pos++)
            {
                if (Header)
                {
                    Plain[Pos] = (byte) (Plain[Pos] ^ prePlain[Pos]);
                }
                else
                {
                    Plain[Pos] = (byte) (Plain[Pos] ^ Out[preCrypt + Pos]);
                }
            }

            var array = Encipher(Plain, Key);
            for (var i = 0; i <= 7; i++)
            {
                Out[Crypt + i] = array[i];
            }

            for (Pos = 0L; Pos <= 7; Pos++)
            {
                Out[Crypt + Pos] = (byte) (Out[Crypt + Pos] ^ prePlain[Pos]);
            }

            Plain.CopyTo(prePlain, 0);
            preCrypt = Crypt;
            Crypt += 8L;
            Pos = 0L;
            Header = false;
        }

        // Token: 0x06000633 RID: 1587 RVA: 0x00025D88 File Offset: 0x00023F88
        private bool Decrypt8Bytes(byte[] arrayIn, long offset = 0L)
        {
            for (Pos = 0L; Pos <= 7; Pos++)
            {
                if (contextStart + Pos > arrayIn.Length - 1)
                {
                    return true;
                }

                prePlain[Pos] = (byte) (prePlain[Pos] ^ arrayIn[offset + Crypt + Pos]);
            }

            try
            {
                prePlain = Decipher(prePlain, Key);
            }
            catch
            {
                return false;
            }

            var num = prePlain.Length - 1;
            contextStart += 8L;
            Crypt += 8L;
            Pos = 0L;
            return true;
        }

        // Token: 0x06000635 RID: 1589 RVA: 0x00025EB0 File Offset: 0x000240B0
        public byte[] Encrypt(byte[] arrayIn, byte[] arrayKey, long offset)
        {
            Plain = new byte[8];
            prePlain = new byte[8];
            Pos = 1L;
            padding = 0L;
            Crypt = preCrypt = 0L;
            Key = arrayKey;
            Header = true;
            Pos = 2L;
            Pos = (arrayIn.Length + 10) % 8;
            if (Pos != 0)
            {
                Pos = 8 - Pos;
            }

            Out = new byte[arrayIn.Length + Pos + 10];
            Plain[0] = (byte) ((Rand() & 0xF8) | Pos);
            for (var i = 1; i <= Pos; i++)
            {
                Plain[i] = (byte) (Rand() & 0xFF);
            }

            Pos++;
            padding = 1L;
            while (padding < 3)
            {
                if (Pos < 8)
                {
                    Plain[Pos] = (byte) (Rand() & 0xFF);
                    padding++;
                    Pos++;
                }
                else if (Pos == 8)
                {
                    Encrypt8Bytes();
                }
            }

            var num = (int) offset;
            long num2 = arrayIn.Length;
            while (num2 > 0)
            {
                if (Pos < 8)
                {
                    Plain[Pos] = arrayIn[num];
                    num++;
                    Pos++;
                    num2--;
                }
                else if (Pos == 8)
                {
                    Encrypt8Bytes();
                }
            }

            padding = 1L;
            while (padding < 9)
            {
                if (Pos < 8)
                {
                    Plain[Pos] = 0;
                    Pos++;
                    padding++;
                }
                else if (Pos == 8)
                {
                    Encrypt8Bytes();
                }
            }

            return Out;
        }

        // Token: 0x06000636 RID: 1590 RVA: 0x000261C0 File Offset: 0x000243C0
        public byte[] Encrypt(byte[] arrayIn, byte[] arrayKey)
        {
            byte[] array = null;
            var num = 0;
            while (array == null && num < 2)
            {
                try
                {
                    array = Encrypt(arrayIn, arrayKey, 0L);
                }
                catch
                {
                }

                num++;
            }

            return array;
        }

        // Token: 0x06000637 RID: 1591 RVA: 0x00026210 File Offset: 0x00024410
        public byte[] Decrypt(byte[] inData, byte[] key)
        {
            var result = new byte[0];
            try
            {
                result = Decrypt(inData, key, 0L);
            }
            catch
            {
            }

            return result;
        }

        // Token: 0x06000638 RID: 1592 RVA: 0x00026250 File Offset: 0x00024450
        public byte[] Decrypt(byte[] arrayIn, byte[] arrayKey, long offset)
        {
            var result = new byte[0];
            if (arrayIn.Length < 16 || arrayIn.Length % 8 != 0)
            {
                return result;
            }

            var array = new byte[offset + 8];
            arrayKey.CopyTo(Key, 0);
            Crypt = preCrypt = 0L;
            prePlain = Decipher(arrayIn, arrayKey, offset);
            Pos = prePlain[0] & 7;
            var num = arrayIn.Length - Pos - 10;
            if (num <= 0)
            {
                return result;
            }

            Out = new byte[num];
            preCrypt = 0L;
            Crypt = 8L;
            contextStart = 8L;
            Pos++;
            padding = 1L;
            while (padding < 3)
            {
                if (Pos < 8)
                {
                    Pos++;
                    padding++;
                }
                else if (Pos == 8)
                {
                    for (var i = 0; i < array.Length; i++)
                    {
                        array[i] = arrayIn[i];
                    }

                    if (!Decrypt8Bytes(arrayIn, offset))
                    {
                        return result;
                    }
                }
            }

            var num2 = 0L;
            while (num != 0)
            {
                if (Pos < 8)
                {
                    Out[num2] = (byte) (array[offset + preCrypt + Pos] ^ prePlain[Pos]);
                    num2++;
                    num--;
                    Pos++;
                }
                else if (Pos == 8)
                {
                    array = arrayIn;
                    preCrypt = Crypt - 8;
                    if (!Decrypt8Bytes(arrayIn, offset))
                    {
                        return result;
                    }
                }
            }

            for (padding = 1L; padding <= 7; padding++)
            {
                if (Pos < 8)
                {
                    if ((array[offset + preCrypt + Pos] ^ prePlain[Pos]) != 0)
                    {
                        return result;
                    }

                    Pos++;
                }
                else if (Pos == 8)
                {
                    for (var i = 0; i < array.Length; i++)
                    {
                        array[i] = arrayIn[i];
                    }

                    preCrypt = Crypt;
                    if (!Decrypt8Bytes(arrayIn, offset))
                    {
                        return result;
                    }
                }
            }

            return Out;
        }
    }
}