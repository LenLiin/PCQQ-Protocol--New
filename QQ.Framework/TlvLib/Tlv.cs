using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.TlvLib
{
    /// <summary>
    ///     TLV data.
    /// </summary>
    public class Tlv
    {
        private readonly int _valueOffset;

        private Tlv(int tag, int length, int valueOffset, byte[] data)
        {
            Tag = tag;
            Length = length;
            Data = data;
            Children = new List<Tlv>();

            _valueOffset = valueOffset;
        }

        /// <summary>
        ///     The raw TLV data.
        /// </summary>
        public byte[] Data { get; }

        /// <summary>
        ///     The raw TLV data.
        /// </summary>
        public string HexData => GetHexString(Data);

        /// <summary>
        ///     The TLV tag.
        /// </summary>
        public int Tag { get; }

        /// <summary>
        ///     The TLV tag.
        /// </summary>
        public string HexTag => Util.NumToHexString(Tag, 4);

        /// <summary>
        ///     The length of the TLV value.
        /// </summary>
        public int Length { get; }

        /// <summary>
        ///     The length of the TLV value.
        /// </summary>
        public string HexLength => Util.NumToHexString(Length, 4);

        /// <summary>
        ///     The TLV value.
        /// </summary>
        public byte[] Value
        {
            get
            {
                var result = new byte[Length];
                Array.Copy(Data, _valueOffset, result, 0, Length);
                return result;
            }
        }

        /// <summary>
        ///     The TLV value.
        /// </summary>
        public string HexValue => GetHexString(Value);

        /// <summary>
        ///     TLV children.
        /// </summary>
        public ICollection<Tlv> Children { get; set; }

        /// <summary>
        ///     Parse TLV data.
        /// </summary>
        /// <param name="tlv">The hex TLV blob.</param>
        /// <returns>A collection of TLVs.</returns>
        public static ICollection<Tlv> ParseTlv(string tlv)
        {
            if (string.IsNullOrWhiteSpace(tlv))
            {
                throw new ArgumentException("tlv");
            }

            return ParseTlv(GetBytes(tlv));
        }

        /// <summary>
        ///     Parse TLV data.
        /// </summary>
        /// <param name="tlv">The byte array TLV blob.</param>
        /// <returns>A collection of TLVs.</returns>
        public static ICollection<Tlv> ParseTlv(byte[] tlv)
        {
            if (tlv == null || tlv.Length == 0)
            {
                throw new ArgumentException("tlv");
            }

            var result = new List<Tlv>();
            ParseTlv(tlv, result);

            return result;
        }

        private static void ParseTlv(byte[] rawTlv, ICollection<Tlv> result)
        {
            for (int i = 0, start = 0; i < rawTlv.Length; start = i)
            {
                // parse Tag
                var constructedTlv = (rawTlv[i] & 0x20) != 0;
                var moreBytes = (rawTlv[i] & 0x1F) == 0x1F;
                while (moreBytes && (rawTlv[++i] & 0x80) != 0)
                {
                    ;
                }

                //i++
                i += 2;

                var tag = GetInt(rawTlv, start, i - start);

                //// parse Length
                //bool multiByteLength = (rawTlv[i] & 0x80) != 0;
                //int length = multiByteLength ? GetInt(rawTlv, i + 1, rawTlv[i] & 0x1F) : rawTlv[i];
                //i = multiByteLength ? i + (rawTlv[i] & 0x1F) + 1 : i + 1;
                i += 2;
                start += 2;
                var length = GetInt(rawTlv, start, i - start);

                i += length;

                var rawData = new byte[i - start];
                Array.Copy(rawTlv, start, rawData, 0, i - start);
                var tlv = new Tlv(tag, length, rawData.Length - length, rawData);
                result.Add(tlv);

                if (constructedTlv)
                {
                    ParseTlv(tlv.Value, tlv.Children);
                }
            }
        }

        private static string GetHexString(byte[] arr)
        {
            var sb = new StringBuilder(arr.Length * 2);
            foreach (var b in arr)
            {
                sb.AppendFormat("{0:X2}", b);
            }

            return sb.ToString();
        }

        private static byte[] GetBytes(string hexString)
        {
            return Enumerable
                .Range(0, hexString.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                .ToArray();
        }

        private static int GetInt(byte[] data, int offset, int length)
        {
            var result = 0;
            for (var i = 0; i < length; i++)
            {
                result = (result << 8) | data[offset + i];
            }

            return result;
        }
    }
}