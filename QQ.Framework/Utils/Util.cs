using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace QQ.Framework.Utils
{
    public static class Util
    {
        private static readonly Encoding DefaultEncoding = Encoding.GetEncoding(QQGlobal.QQCharsetDefault);
        private static readonly DateTime BaseDateTime = DateTime.Parse("1970-1-01 00:00:00.000");

        public static Random Random = new Random();

        /// <summary>
        ///     把字节数组从offset开始的len个字节转换成一个unsigned int，
        ///     <remark>2008-02-15 14:47 </remark>
        /// </summary>
        /// <param name="inData">字节数组</param>
        /// <param name="offset">从哪里开始转换.</param>
        /// <param name="len">转换长度, 如果len超过8则忽略后面的.</param>
        /// <returns></returns>
        public static uint GetUInt(byte[] inData, int offset, int len)
        {
            uint ret = 0;
            int end;
            if (len > 8)
            {
                end = offset + 8;
            }
            else
            {
                end = offset + len;
            }

            for (var i = 0; i < end; i++)
            {
                ret <<= 8;
                ret |= inData[i];
            }

            return ret;
        }

        public static string LongToHexString(long num)
        {
            return $"{num:X}";
        }

        /// <summary>
        ///     字符串转byte[]数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(string hexString)
        {
            hexString = hexString.Replace(" ", "").Replace("\n", "");
            if (hexString.Length % 2 != 0)
            {
                hexString += " ";
            }

            var array = new byte[hexString.Length / 2];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return array;
        }

        public static byte[] Int64_to_4byte(long paramLong)
        {
            byte[] array =
            {
                0, 0, 0, (byte) (int) paramLong
            };
            array[2] = (byte) (int) (paramLong >> 8);
            array[1] = (byte) (int) (paramLong >> 16);
            array[0] = (byte) (int) (paramLong >> 24);
            return array;
        }

        public static string ConvertStringToHex(string text, string separator = null)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            return ToHex(bytes);
        }

        public static string GetQQNum(string six)
        {
            return Convert.ToInt64(six.Replace(" ", ""), 16).ToString();
        }

        public static ulong GetQQNumRetUint(string six)
        {
            return Convert.ToUInt64(six.Replace(" ", ""), 16);
        }

        /// <summary>
        ///     转换hex  长度不够前置补0
        /// </summary>
        /// <param name="qq"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string NumToHexString(long qq, int length = 8)
        {
            var text = Convert.ToString(qq, 16);
            if (text.Length == length)
            {
                return text;
            }

            if (text.Length > length)
            {
                return null;
            }

            var num = length - text.Length;
            var str = "";
            for (var i = 0; i < num; i++)
            {
                str += "0";
            }

            text = (str + text).ToUpper();
            var stringBuilder = new StringBuilder();
            for (var j = 0; j < text.Length; j++)
            {
                stringBuilder.Append(text[j]);
                if ((j + 1) % 2 == 0)
                {
                    stringBuilder.Append(" ");
                }
            }

            return stringBuilder.ToString();
        }

        public static string ConvertHexToString(string hexValue)
        {
            var bytes = HexStringToByteArray(hexValue);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        ///     根据某种编码方式将字节数组转换成字符串
        /// </summary>
        /// <param name="b">字节数组</param>
        /// <param name="encoding">encoding 编码方式</param>
        /// <returns> 如果encoding不支持，返回一个缺省编码的字符串</returns>
        public static string GetString(byte[] b, string encoding = QQGlobal.QQCharsetDefault)
        {
            try
            {
                return Encoding.GetEncoding(encoding).GetString(b);
            }
            catch
            {
                return Encoding.Default.GetString(b);
            }
        }

        /// <summary>
        ///     根据某种编码方式将字节数组转换成字符串
        ///     <remark>2008-02-22 </remark>
        /// </summary>
        /// <param name="b">The b.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="len">The len.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static string GetString(byte[] b, int offset, int len, string encoding = QQGlobal.QQCharsetDefault)
        {
            var temp = new byte[len];
            Array.Copy(b, offset, temp, 0, len);
            return GetString(temp, encoding);
        }

        /// <summary>
        ///     把字符串转换成int
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="defaultValue">如果转换失败，返回这个值</param>
        /// <returns></returns>
        public static int GetInt(string s, int defaultValue)
        {
            return int.TryParse(s, out var value) ? value : defaultValue;
        }

        /// <summary>
        ///     字符串转二进制字数组
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static byte[] GetBytes(string s)
        {
            return DefaultEncoding.GetBytes(s);
        }

        /// <summary>
        ///     一个随机产生的密钥字节数组
        /// </summary>
        /// <returns></returns>
        public static byte[] RandomKey()
        {
            var key = new byte[QQGlobal.QQLengthKey];
            new Random().NextBytes(key);
            return key;
        }

        /// <summary>
        ///     一个随机产生的密钥字节数组
        /// </summary>
        /// <returns></returns>
        public static byte[] RandomKey(int length)
        {
            var key = new byte[length];
            new Random().NextBytes(key);
            return key;
        }


        /// <summary>
        ///     用于代替 System.currentTimeMillis()
        ///     <remark>2008-02-29 </remark>
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static long GetTimeMillis(DateTime dateTime)
        {
            return (long) (dateTime - BaseDateTime).TotalMilliseconds;
        }

        public static long GetTimeSeconds(DateTime dateTime)
        {
            return (long) (dateTime - BaseDateTime).TotalSeconds;
        }

        /// <summary>
        ///     根据服务器返回的毫秒表示的日期，获得实际的日期
        ///     Gets the date time from millis.
        ///     似乎服务器返回的日期要加上8个小时才能得到正确的 +8 时区的登录时间
        /// </summary>
        /// <param name="millis">The millis.</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromMillis(long millis)
        {
            return BaseDateTime.AddTicks(millis * TimeSpan.TicksPerSecond).AddHours(8);
        }

        /// <summary>
        ///     判断IP是否全0
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <returns></returns>
        public static bool IsIPZero(byte[] ip)
        {
            return ip.All(t => t == 0);
        }

        /// <summary>
        ///     ip的字节数组形式转为字符串形式的ip
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <returns></returns>
        public static string GetIpStringFromBytes(byte[] ip)
        {
            return $"{ip[0]}.{ip[1]}.{ip[2]}.{ip[3]}";
        }

        public static string ToHex(byte[] bs, string newLine = "", string format = "{0} ")
        {
            var num = 0;
            var stringBuilder = new StringBuilder();
            foreach (var b in bs)
            {
                if (num++ % 16 == 0)
                {
                    stringBuilder.Append(newLine);
                }

                stringBuilder.AppendFormat(format, b.ToString("X2"));
            }

            return stringBuilder.ToString().Trim();
        }

        public static byte[] IPStringToByteArray(string ip)
        {
            var array = new byte[4];
            var array2 = ip.Split('.');
            if (array2.Length == 4)
            {
                for (var i = 0; i < 4; i++)
                {
                    array[i] = (byte) int.Parse(array2[i]);
                }
            }

            return array;
        }

        /// <summary>
        ///     获取本地外网 IP
        /// </summary>
        /// <returns></returns>
        public static string GetExternalIp()
        {
            var mc = Regex.Match(
                new HttpClient().GetStringAsync("http://www.net.cn/static/customercare/yourip.asp").Result,
                @"您的本地上网IP是：<h2>(\d+\.\d+\.\d+\.\d+)</h2>");
            if (mc.Success && mc.Groups.Count > 1)
            {
                return mc.Groups[1].Value;
            }

            throw new Exception("获取IP失败");
        }

        /// <summary>
        ///     根据域名获取IP
        /// </summary>
        /// <param name="hostname">域名</param>
        /// <returns></returns>
        public static string GetHostAddresses(string hostname)
        {
            var ips = Dns.GetHostAddresses(hostname);

            return ips[0].ToString();
        }

        public static string GetBkn(string skey)
        {
            var num = 5381;
            for (var i = 0; i <= skey.Length - 1; i++)
            {
                num += (num << 5) + Convert.ToInt32(char.Parse(skey.Substring(i, 1)));
            }

            return (num & 0x7FFFFFFF).ToString();
        }

        public static string GET_GTK(string skey)
        {
            var arg = "tencentQQVIP123443safde&!%^%1282";
            var list = new List<int>();
            var num = 5381;
            list.Add(172192);
            var i = 0;
            for (var length = skey.Length; i < length; i++)
            {
                int num2 = Encoding.UTF8.GetBytes(skey)[i];
                list.Add((num << 5) + num2);
                num = num2;
            }

            var stringBuilder = new StringBuilder();
            for (i = 0; i < list.Count; i++)
            {
                stringBuilder.Append(list[i].ToString());
            }

            return QQTea.Md5(stringBuilder + arg);
        }

        public static void TalkQQ(uint uin)
        {
            TalkQQ(uin.ToString());
        }

        public static void TalkQQ(string uin)
        {
            try
            {
                var fileName = $"tencent://message/?uin={uin}&Site=qq&Menu=yes";
                Process.Start(fileName);
            }
            catch
            {
            }
        }

        /// <summary>
        ///     一个特殊的加密
        /// </summary>
        public static string PB_toLength(long d)
        {
            var binary = Convert.ToString(d, 2); //转换length为二级制
            var temp = "";
            while (!string.IsNullOrEmpty(binary))
            {
                var binary1 = "0000000" + binary;
                temp = temp + "1" + binary1.Substring(binary1.Length - 7, 7);
                if (binary.Length >= 7)
                {
                    binary = binary.Substring(0, binary.Length - 7);
                }
                else
                {
                    //temp = temp + "0" + binary;
                    break;
                }
            }

            var temp1 = temp.Substring(temp.Length - 7, 7);
            temp = temp.Substring(0, temp.Length - 8) + "0" + temp1;
            return LongToHexString(Convert.ToInt64(temp, 2));
        }

        public static byte[] ToBytesArray(this Stream stream)
        {
            return ((MemoryStream) stream).ToArray();
        }

        /// <summary>
        ///     获取文件Md5
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                var file = new FileStream(fileName, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                var retVal = md5.ComputeHash(file);
                file.Close();

                var sb = new StringBuilder();
                foreach (var t in retVal)
                {
                    sb.Append(t.ToString("x2"));
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        public static string GetMD5ToGuidHashFromFile(string fileName)
        {
            var md5 = GetMD5HashFromFile(fileName);
            return "{" +
                   md5.Substring(0, 8) + "-" +
                   md5.Substring(8, 4) + "-" +
                   md5.Substring(12, 4) + "-" +
                   md5.Substring(16, 4) + "-" +
                   md5.Substring(20) + "-" +
                   "}";
        }

        /// <summary>
        ///     实体转化为字节数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static byte[] ModelToByte<T>(T t)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryReader = new BinaryReader(memoryStream);
                memoryStream.Position = 0L;
                return binaryReader.ReadBytes((int) memoryStream.Length);
            }
        }

        #region TLV专属操作方法

        public static void int16_to_buf(byte[] tempByteArray, long index, long num)
        {
            tempByteArray[index] = (byte) ((num & 0xff000000) >> 24);
            tempByteArray[index + 1] = (byte) ((num & 0x00ff0000) >> 16);
            tempByteArray[index + 2] = (byte) ((num & 0x0000ff00) >> 8);
            tempByteArray[index + 3] = (byte) (num & 0x000000ff);
        }

        public static int buf_to_int16(byte[] tempByteArray, long index)
        {
            return (tempByteArray[index] << 24) | (tempByteArray[index + 1] << 16) |
                   (tempByteArray[index + 2] << 8) | tempByteArray[index + 3];
        }

        public static long CurrentTimeMillis()
        {
            return GetTimeMillis(DateTime.Now);
        }

        internal static string MapPath(string directory)
        {
            return AppDomain.CurrentDomain.BaseDirectory + "//" + directory;
        }

        #endregion

        #region BinaryWriter和BinaryReader扩展方法

        /*
           协议使用的是大端序(Big-Endian)，而BinaryWriter和BinaryReader使用的是小端序。
           因此超过一个字节结构(char, ushort, int, long等)的读取和写入需要自定义。
           byte[]的读取/写入方式不需要更改。
        */
        public static void BeWrite(this BinaryWriter bw, ushort v)
        {
            bw.Write(BitConverter.GetBytes(v).Reverse().ToArray());
        }

        public static void BeWrite(this BinaryWriter bw, DateTime v)
        {
            bw.BeWrite(GetTimeSeconds(v));
        }

        public static void BeWrite(this BinaryWriter bw, char v)
        {
            bw.Write(BitConverter.GetBytes((ushort) v).Reverse().ToArray());
        }

        public static void BeWrite(this BinaryWriter bw, int v)
        {
            bw.Write(BitConverter.GetBytes(v).Reverse().ToArray());
        }

        public static void BeUshortWrite(this BinaryWriter bw, ushort v)
        {
            bw.BeWrite(v);
        }

        // 注意: 此处的long和ulong均为四个字节，而不是八个。
        public static void BeWrite(this BinaryWriter bw, long v)
        {
            bw.Write(BitConverter.GetBytes((uint) v).Reverse().ToArray());
        }

        public static void BeWrite(this BinaryWriter bw, ulong v)
        {
            bw.Write(BitConverter.GetBytes((uint) v).Reverse().ToArray());
        }

        /// <summary>
        ///     写入一串秘钥（因为结构需要前置秘钥长度）
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="v"></param>
        public static void WriteKey(this BinaryWriter bw, byte[] v)
        {
            bw.BeWrite((ushort) v.Length);
            bw.Write(v);
        }

        public static List<byte[]> WriteSnippet(TextSnippet snippet, int length)
        {
            // TODO: 富文本支持
            var ret = new List<byte[]>();
            var bw = new BinaryWriter(new MemoryStream());
            switch (snippet.Type)
            {
                case MessageType.Normal:
                {
                    if (length + 6 >= 699) // 数字应该稍大点，但是我不清楚具体是多少
                    {
                        length = 0;
                        ret.Add(new byte[0]);
                    }

                    bw.BaseStream.Position = 6;
                    foreach (var chr in snippet.Content)
                    {
                        var bytes = Encoding.UTF8.GetBytes(chr.ToString());
                        // 705 = 699 + 6个byte: (byte + short + byte + short)
                        if (length + bw.BaseStream.Length + bytes.Length > 705)
                        {
                            var pos = bw.BaseStream.Position;
                            bw.BaseStream.Position = 0;
                            bw.Write(new byte[] {0x01});
                            bw.BeWrite((ushort) (pos - 3)); // 本来是+3和0的，但是提前预留了6个byte给它们，所以变成了-3和-6。下同理。
                            bw.Write(new byte[] {0x01});
                            bw.BeWrite((ushort) (pos - 6));
                            bw.BaseStream.Position = pos;
                            ret.Add(bw.BaseStream.ToBytesArray());
                            bw = new BinaryWriter(new MemoryStream());
                            bw.BaseStream.Position = 6;
                            length = 0;
                        }

                        bw.Write(bytes);
                    }

                    // 在最后一段的开头补充结构 
                    {
                        var pos = bw.BaseStream.Position;
                        bw.BaseStream.Position = 0;
                        bw.Write(new byte[] {0x01});
                        bw.BeWrite((ushort) (pos - 3));
                        bw.Write(new byte[] {0x01});
                        bw.BeWrite((ushort) (pos - 6));
                        bw.BaseStream.Position = pos;
                    }
                    break;
                }
                case MessageType.At:
                    break;
                case MessageType.Emoji:
                {
                    if (length + 12 > 699)
                    {
                        ret.Add(new byte[0]);
                    }

                    var faceIndex = Convert.ToByte(snippet.Content);
                    if (faceIndex > 199)
                    {
                        faceIndex = 0;
                    }

                    bw.Write(new byte[] {0x02, 0x00, 0x14, 0x01, 0x00, 0x01});
                    bw.Write(faceIndex);
                    bw.Write(new byte[] {0xFF, 0x00, 0x02, 0x14});
                    bw.Write((byte) (faceIndex + 65));
                    bw.Write(new byte[] {0x0B, 0x00, 0x08, 0x00, 0x01, 0x00, 0x04, 0x52, 0xCC, 0x85, 0x50});
                    break;
                }
                case MessageType.Picture:
                    break;
                case MessageType.Xml:
                    break;
                case MessageType.Json:
                    break;
                case MessageType.Shake:
                    break;
                case MessageType.Audio:
                    break;
                case MessageType.Video:
                    break;
                case MessageType.ExitGroup:
                    break;
                case MessageType.GetGroupImformation:
                    break;
                case MessageType.AddGroup:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (bw.BaseStream.Position != 0)
            {
                ret.Add(bw.BaseStream.ToBytesArray());
            }

            return ret;
        }

        public static List<byte[]> WriteRichtext(Richtext richtext)
        {
            if (richtext.Snippets.Count > 1)
            {
                if (!richtext.Snippets.TrueForAll(s =>
                    s.Type == MessageType.Normal || s.Type == MessageType.At || s.Type == MessageType.Emoji ||
                    s.Type == MessageType.Picture))
                {
                    throw new NotSupportedException("富文本中包含多个非聊天代码");
                }
            }

            // TODO: 富文本支持
            var ret = new List<byte[]>();
            var bw = new BinaryWriter(new MemoryStream());
            foreach (var snippet in richtext.Snippets)
            {
                var list = WriteSnippet(snippet, (int) bw.BaseStream.Position);
                for (var i = 0; i < list.Count; i++)
                {
                    bw.Write(list[i]);
                    // 除最后一个以外别的都开新的包
                    //   如果有多个，那前几个一定是太长了被分段了，所以开新的包
                    //   如果只有一个/是最后一个，那就不开
                    if (i == list.Count - 1)
                    {
                        break;
                    }

                    ret.Add(bw.BaseStream.ToBytesArray());
                    bw = new BinaryWriter(new MemoryStream());
                }
            }

            ret.Add(bw.BaseStream.ToBytesArray());
            return ret;
        }

        public static char BeReadChar(this BinaryReader br)
        {
            return (char) br.BeReadUInt16();
        }

        public static ushort BeReadUInt16(this BinaryReader br)
        {
            return (ushort) ((br.ReadByte() << 8) + br.ReadByte());
        }

        public static int BeReadInt32(this BinaryReader br)
        {
            return (br.ReadByte() << 24) | (br.ReadByte() << 16) | (br.ReadByte() << 8) | br.ReadByte();
        }

        public static uint BeReadUInt32(this BinaryReader br)
        {
            return (uint) ((br.ReadByte() << 24) | (br.ReadByte() << 16) | (br.ReadByte() << 8) | br.ReadByte());
        }

        public static Richtext ReadRichtext(this BinaryReader br)
        {
            // TODO: 解析富文本
            // 目前进度: 仅读取第一部分
            return Richtext.Parse(br.ReadBytes(br.BeReadChar()));
        }

        #endregion
    }
}