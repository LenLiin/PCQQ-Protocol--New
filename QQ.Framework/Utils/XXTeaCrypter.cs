namespace QQ.Framework.Utils
{
    public static class XXTeaCrypter
    {
        public static byte[] Encrypt(byte[] data, byte[] key)
        {
            if (data.Length == 0)
            {
                return data;
            }

            return ToByteArray(Encrypt(ToUInt32Array(data, true), ToUInt32Array(key, false)), false);
        }

        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            try
            {
                return data.Length == 0
                    ? data
                    : ToByteArray(Decrypt(ToUInt32Array(data, false), ToUInt32Array(key, false)), true);
            }
            catch
            {
                return null;
            }
        }

        public static uint[] Encrypt(uint[] v, uint[] k)
        {
            var num = v.Length - 1;
            if (num < 1)
            {
                return v;
            }

            if (k.Length < 4)
            {
                var array = new uint[4];
                k.CopyTo(array, 0);
                k = array;
            }

            var num2 = v[num];
            var num9 = v[0];
            var num3 = 2654435769u;
            var num4 = 0u;
            var num5 = 6 + 52 / (num + 1);
            while (num5-- > 0)
            {
                num4 += num3;
                var num7 = (num4 >> 2) & 3;
                int i;
                uint num8;
                for (i = 0; i < num; i++)
                {
                    num8 = v[i + 1];
                    num2 = v[i] += (((num2 >> 5) ^ (num8 << 2)) + ((num8 >> 3) ^ (num2 << 4))) ^
                                   ((num4 ^ num8) + (k[(i & 3) ^ num7] ^ num2));
                }

                num8 = v[0];
                num2 = v[num] += (((num2 >> 5) ^ (num8 << 2)) + ((num8 >> 3) ^ (num2 << 4))) ^
                                 ((num4 ^ num8) + (k[(i & 3) ^ num7] ^ num2));
            }

            return v;
        }

        public static uint[] Decrypt(uint[] v, uint[] k)
        {
            var num = v.Length - 1;
            if (num < 1)
            {
                return v;
            }

            if (k.Length < 4)
            {
                var array = new uint[4];
                k.CopyTo(array, 0);
                k = array;
            }

            var num9 = v[num];
            var num2 = v[0];
            var num3 = 2654435769u;
            var num4 = 6 + 52 / (num + 1);
            for (var num5 = (uint) (num4 * num3); num5 != 0; num5 -= num3)
            {
                var num6 = (num5 >> 2) & 3;
                int num7;
                uint num8;
                for (num7 = num; num7 > 0; num7--)
                {
                    num8 = v[num7 - 1];
                    num2 = v[num7] -= (((num8 >> 5) ^ (num2 << 2)) + ((num2 >> 3) ^ (num8 << 4))) ^
                                      ((num5 ^ num2) + (k[(num7 & 3) ^ num6] ^ num8));
                }

                num8 = v[num];
                num2 = v[0] -= (((num8 >> 5) ^ (num2 << 2)) + ((num2 >> 3) ^ (num8 << 4))) ^
                               ((num5 ^ num2) + (k[(num7 & 3) ^ num6] ^ num8));
            }

            return v;
        }

        private static uint[] ToUInt32Array(byte[] data, bool includeLength)
        {
            var num = (data.Length & 3) == 0 ? data.Length >> 2 : (data.Length >> 2) + 1;
            uint[] array;
            if (includeLength)
            {
                array = new uint[num + 1];
                array[num] = (uint) data.Length;
            }
            else
            {
                array = new uint[num];
            }

            num = data.Length;
            for (var i = 0; i < num; i++)
            {
                array[i >> 2] |= (uint) (data[i] << ((i & 3) << 3));
            }

            return array;
        }

        private static byte[] ToByteArray(uint[] data, bool includeLength)
        {
            var num = !includeLength ? data.Length << 2 : (int) data[data.Length - 1];
            var array = new byte[num];
            for (var i = 0; i < num; i++)
            {
                array[i] = (byte) (data[i >> 2] >> ((i & 3) << 3));
            }

            return array;
        }
    }
}