namespace QQ.Framework.Utils
{
    public class TeaCrypter
    {
        // Token: 0x04000334 RID: 820
        private long _contextStart;

        // Token: 0x04000335 RID: 821
        private long _crypt;

        // Token: 0x04000336 RID: 822
        private long _preCrypt;

        // Token: 0x04000337 RID: 823
        private bool _header;

        // Token: 0x04000338 RID: 824
        private byte[] _key = new byte[16];

        // Token: 0x04000339 RID: 825
        private byte[] _out;

        // Token: 0x0400033A RID: 826
        private long _padding;

        // Token: 0x0400033B RID: 827
        private byte[] _plain;

        // Token: 0x0400033C RID: 828
        private long _pos;

        // Token: 0x0400033D RID: 829
        private byte[] _prePlain;

        // Token: 0x06000629 RID: 1577 RVA: 0x000256F4 File Offset: 0x000238F4
        public byte[] MD5(byte[] data)
        {
            return System.Security.Cryptography.MD5.Create().ComputeHash(data);
        }

        // Token: 0x0600062A RID: 1578 RVA: 0x00025710 File Offset: 0x00023910
        private byte[] CopyMemory(byte[] arr, int arrIndex, long input)
        {
            if (arrIndex + 4 > arr.Length)
            {
                return arr;
            }

            arr[arrIndex + 3] = (byte) ((input & 4278190080u) >> 24);
            arr[arrIndex + 2] = (byte) ((input & 0xFF0000) >> 16);
            arr[arrIndex + 1] = (byte) ((input & 0xFF00) >> 8);
            arr[arrIndex] = (byte) (input & 0xFF);
            arr[arrIndex] &= byte.MaxValue;
            arr[arrIndex + 1] &= byte.MaxValue;
            arr[arrIndex + 2] &= byte.MaxValue;
            arr[arrIndex + 3] &= byte.MaxValue;
            return arr;
        }

        private long CopyMemory(long Out, byte[] arr, int arrIndex)
        {
            if (arrIndex + 4 > arr.Length)
            {
                return Out;
            }

            long num = arr[arrIndex + 3] << 24;
            long num2 = arr[arrIndex + 2] << 16;
            long num3 = arr[arrIndex + 1] << 8;
            long num4 = arr[arrIndex];
            var num5 = num | num2 | num3 | num4;
            return num5 & uint.MaxValue;
        }

        private long GetUnsignedInt(byte[] arrayIn, int offset, int len)
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
            var num3 = GetUnsignedInt(arrayIn, (int) offset, 4);
            var num4 = GetUnsignedInt(arrayIn, (int) offset + 4, 4);
            var unsignedInt = GetUnsignedInt(arrayKey, 0, 4);
            var unsignedInt2 = GetUnsignedInt(arrayKey, 4, 4);
            var unsignedInt3 = GetUnsignedInt(arrayKey, 8, 4);
            var unsignedInt4 = GetUnsignedInt(arrayKey, 12, 4);
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
            var num3 = GetUnsignedInt(arrayIn, (int) offset, 4);
            var num4 = GetUnsignedInt(arrayIn, (int) offset + 4, 4);
            var unsignedInt = GetUnsignedInt(arrayKey, 0, 4);
            var unsignedInt2 = GetUnsignedInt(arrayKey, 4, 4);
            var unsignedInt3 = GetUnsignedInt(arrayKey, 8, 4);
            var unsignedInt4 = GetUnsignedInt(arrayKey, 12, 4);
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
            for (_pos = 0L; _pos < 8; _pos++)
            {
                if (_header)
                {
                    _plain[_pos] = (byte) (_plain[_pos] ^ _prePlain[_pos]);
                }
                else
                {
                    _plain[_pos] = (byte) (_plain[_pos] ^ _out[_preCrypt + _pos]);
                }
            }

            var array = Encipher(_plain, _key);
            for (var i = 0; i <= 7; i++)
            {
                _out[_crypt + i] = array[i];
            }

            for (_pos = 0L; _pos <= 7; _pos++)
            {
                _out[_crypt + _pos] = (byte) (_out[_crypt + _pos] ^ _prePlain[_pos]);
            }

            _plain.CopyTo(_prePlain, 0);
            _preCrypt = _crypt;
            _crypt += 8L;
            _pos = 0L;
            _header = false;
        }

        // Token: 0x06000633 RID: 1587 RVA: 0x00025D88 File Offset: 0x00023F88
        private bool Decrypt8Bytes(byte[] arrayIn, long offset = 0L)
        {
            for (_pos = 0L; _pos <= 7; _pos++)
            {
                if (_contextStart + _pos > arrayIn.Length - 1)
                {
                    return true;
                }

                _prePlain[_pos] = (byte) (_prePlain[_pos] ^ arrayIn[offset + _crypt + _pos]);
            }

            try
            {
                _prePlain = Decipher(_prePlain, _key);
            }
            catch
            {
                return false;
            }

            var num = _prePlain.Length - 1;
            _contextStart += 8L;
            _crypt += 8L;
            _pos = 0L;
            return true;
        }

        // Token: 0x06000635 RID: 1589 RVA: 0x00025EB0 File Offset: 0x000240B0
        public byte[] Encrypt(byte[] arrayIn, byte[] arrayKey, long offset)
        {
            _plain = new byte[8];
            _prePlain = new byte[8];
            _pos = 1L;
            _padding = 0L;
            _crypt = _preCrypt = 0L;
            _key = arrayKey;
            _header = true;
            _pos = 2L;
            _pos = (arrayIn.Length + 10) % 8;
            if (_pos != 0)
            {
                _pos = 8 - _pos;
            }

            _out = new byte[arrayIn.Length + _pos + 10];
            _plain[0] = (byte) ((Rand() & 0xF8) | _pos);
            for (var i = 1; i <= _pos; i++)
            {
                _plain[i] = (byte) (Rand() & 0xFF);
            }

            _pos++;
            _padding = 1L;
            while (_padding < 3)
            {
                if (_pos < 8)
                {
                    _plain[_pos] = (byte) (Rand() & 0xFF);
                    _padding++;
                    _pos++;
                }
                else if (_pos == 8)
                {
                    Encrypt8Bytes();
                }
            }

            var num = (int) offset;
            long num2 = arrayIn.Length;
            while (num2 > 0)
            {
                if (_pos < 8)
                {
                    _plain[_pos] = arrayIn[num];
                    num++;
                    _pos++;
                    num2--;
                }
                else if (_pos == 8)
                {
                    Encrypt8Bytes();
                }
            }

            _padding = 1L;
            while (_padding < 9)
            {
                if (_pos < 8)
                {
                    _plain[_pos] = 0;
                    _pos++;
                    _padding++;
                }
                else if (_pos == 8)
                {
                    Encrypt8Bytes();
                }
            }

            return _out;
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
            arrayKey.CopyTo(_key, 0);
            _crypt = _preCrypt = 0L;
            _prePlain = Decipher(arrayIn, arrayKey, offset);
            _pos = _prePlain[0] & 7;
            var num = arrayIn.Length - _pos - 10;
            if (num <= 0)
            {
                return result;
            }

            _out = new byte[num];
            _preCrypt = 0L;
            _crypt = 8L;
            _contextStart = 8L;
            _pos++;
            _padding = 1L;
            while (_padding < 3)
            {
                if (_pos < 8)
                {
                    _pos++;
                    _padding++;
                }
                else if (_pos == 8)
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
                if (_pos < 8)
                {
                    _out[num2] = (byte) (array[offset + _preCrypt + _pos] ^ _prePlain[_pos]);
                    num2++;
                    num--;
                    _pos++;
                }
                else if (_pos == 8)
                {
                    array = arrayIn;
                    _preCrypt = _crypt - 8;
                    if (!Decrypt8Bytes(arrayIn, offset))
                    {
                        return result;
                    }
                }
            }

            for (_padding = 1L; _padding <= 7; _padding++)
            {
                if (_pos < 8)
                {
                    if ((array[offset + _preCrypt + _pos] ^ _prePlain[_pos]) != 0)
                    {
                        return result;
                    }

                    _pos++;
                }
                else if (_pos == 8)
                {
                    for (var i = 0; i < array.Length; i++)
                    {
                        array[i] = arrayIn[i];
                    }

                    _preCrypt = _crypt;
                    if (!Decrypt8Bytes(arrayIn, offset))
                    {
                        return result;
                    }
                }
            }

            return _out;
        }
    }
}