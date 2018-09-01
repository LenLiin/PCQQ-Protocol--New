using System;
using System.Runtime.InteropServices;

namespace QQ.Framework.Utils
{
    /// <summary>
    ///     ECDH操作类
    /// </summary>
    public class ECDHCrypter
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr ECDH_KDF([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
            byte[] pin, int inlen, IntPtr pout, ref int outlen);

        public static bool HasInited;

        static ECDHCrypter()
        {
            try
            {
                var value = EC_KEY_new_by_curve_name(711);
                if (value != IntPtr.Zero)
                {
                    HasInited = true;
                }
            }
            catch
            {
                HasInited = false;
            }
        }

        [DllImport("libeay32", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr EC_KEY_new_by_curve_name(int nid);

        [DllImport("libeay32", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr EC_KEY_get0_group(IntPtr key);

        [DllImport("libeay32", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr EC_POINT_new(IntPtr group);

        [DllImport("libeay32", CallingConvention = CallingConvention.Cdecl)]
        public static extern int EC_KEY_generate_key(IntPtr key);

        [DllImport("libeay32", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr EC_KEY_get0_public_key(IntPtr key);

        [DllImport("libeay32", CallingConvention = CallingConvention.Cdecl)]
        public static extern void EC_KEY_free(IntPtr key);

        [DllImport("libeay32", CallingConvention = CallingConvention.Cdecl)]
        public static extern void EC_GROUP_free(IntPtr group);

        [DllImport("libeay32", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ECDH_compute_key(byte[] pout, int outlen, IntPtr pub_key, IntPtr ecdh, ECDH_KDF kdf);

        [DllImport("libeay32", CallingConvention = CallingConvention.Cdecl)]
        public static extern int EC_POINT_point2oct(IntPtr group, IntPtr p, int form, byte[] buf, int len, IntPtr ctx);

        [DllImport("libeay32", CallingConvention = CallingConvention.Cdecl)]
        public static extern int EC_POINT_oct2point(IntPtr group, IntPtr p, byte[] buf, int len, IntPtr ctx);

        public static ECDH_struct GenKeys(int curveID)
        {
            var result = default(ECDH_struct);
            try
            {
                if (curveID <= 0 || curveID == 711)
                {
                    byte[] array =
                    {
                        4,
                        146,
                        141,
                        136,
                        80,
                        103,
                        48,
                        136,
                        179,
                        67,
                        38,
                        78,
                        12,
                        107,
                        172,
                        184,
                        73,
                        109,
                        105,
                        119,
                        153,
                        243,
                        114,
                        17,
                        222,
                        178,
                        91,
                        183,
                        57,
                        6,
                        203,
                        8,
                        159,
                        234,
                        150,
                        57,
                        180,
                        224,
                        38,
                        4,
                        152,
                        181,
                        26,
                        153,
                        45,
                        80,
                        129,
                        61,
                        168
                    };
                    var array2 = new byte[25];
                    var array3 = new byte[16];
                    var intPtr = EC_KEY_new_by_curve_name(711);
                    if (intPtr != IntPtr.Zero)
                    {
                        var intPtr2 = EC_KEY_get0_group(intPtr);
                        if (intPtr2 != IntPtr.Zero)
                        {
                            var intPtr3 = EC_POINT_new(intPtr2);
                            if (EC_KEY_generate_key(intPtr) == 1)
                            {
                                var p = EC_KEY_get0_public_key(intPtr);
                                var num = EC_POINT_point2oct(intPtr2, p, 2, array2, 64, IntPtr.Zero);
                                var num2 = EC_POINT_oct2point(intPtr2, intPtr3, array, array.Length, IntPtr.Zero);
                                if (num2 == 1)
                                {
                                    num = ECDH_compute_key(array3, 64, intPtr3, intPtr, null);
                                    if (num > 0)
                                    {
                                        result.EC_publickey = array2;
                                        result.EC_sharekey = array3;
                                    }
                                }
                            }
                        }

                        EC_GROUP_free(intPtr2);
                    }
                }
                else if (curveID == 708)
                {
                    byte[] array =
                    {
                        4,
                        138,
                        208,
                        197,
                        239,
                        84,
                        129,
                        144,
                        209,
                        217,
                        66,
                        85,
                        101,
                        41,
                        239,
                        111,
                        198,
                        218,
                        152,
                        89,
                        228,
                        47,
                        13,
                        67,
                        86,
                        52,
                        77,
                        34,
                        198,
                        119,
                        172,
                        144,
                        2,
                        byte.MaxValue,
                        39,
                        43,
                        226,
                        184,
                        64,
                        154,
                        60
                    };
                    var array2 = new byte[21];
                    var array3 = new byte[16];
                    var intPtr = EC_KEY_new_by_curve_name(708);
                    if (intPtr != IntPtr.Zero)
                    {
                        var intPtr2 = EC_KEY_get0_group(intPtr);
                        if (intPtr2 != IntPtr.Zero)
                        {
                            var intPtr3 = EC_POINT_new(intPtr2);
                            if (EC_KEY_generate_key(intPtr) == 1)
                            {
                                var p = EC_KEY_get0_public_key(intPtr);
                                var num = EC_POINT_point2oct(intPtr2, p, 2, array2, 64, IntPtr.Zero);
                                var num2 = EC_POINT_oct2point(intPtr2, intPtr3, array, array.Length, IntPtr.Zero);
                                if (num2 == 1)
                                {
                                    num = ECDH_compute_key(array3, 41, intPtr3, intPtr, null);
                                    if (num > 0)
                                    {
                                        result.EC_publickey = array2;
                                        result.EC_sharekey = array3;
                                    }
                                }
                            }
                        }

                        EC_GROUP_free(intPtr2);
                    }

                    EC_KEY_free(intPtr);
                }
            }
            catch
            {
            }

            return result;
        }
    }

    public struct ECDH_struct
    {
        public byte[] EC_publickey { get; set; }

        public byte[] EC_sharekey { get; set; }
    }
}