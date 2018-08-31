#region 版权声明
/**
 * 版权声明：QQ.Framework是基于LumaQQ分析的QQ协议，将其部分代码进行修改和翻译为.NET版本，并且继续使用LumaQQ的开源协议。
 * 本人没有对其核心协议进行改动， 也没有与腾讯公司的QQ软件有直接联系，请尊重LumaQQ作者Luma的著作权和版权声明。
 * 同时在使用此开发包前请自行协调好多方面关系，本人不享受和承担由此产生的任何权利以及任何法律责任。
 * 
 * 作者：阿不
 * 博客：http://hjf1223.cnblogs.com
 * Email：hjf1223@gmail.com
 * LumaQQ：http://lumaqq.linuxsir.org 
 * LumaQQ - Java QQ Client
 * 
 * Copyright (C) 2004 luma <stubma@163.com>
 * 
 * LumaQQ - For .NET QQClient
 * Copyright (C) 2008 阿不<hjf1223@gmail.com>
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
 */
#endregion
using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Net;
using System.Text.RegularExpressions;

namespace QQ.Framework.Utils
{
    public static class Util
    {
        static Encoding DefaultEncoding = Encoding.GetEncoding(QQGlobal.QQ_CHARSET_DEFAULT);
        static DateTime baseDateTime = DateTime.Parse("1970-1-01 00:00:00.000");


        public static Random Random = new Random();
        /// <summary>
        /// 把字节数组从offset开始的len个字节转换成一个unsigned int，
        /// 	<remark>2008-02-15 14:47 </remark>
        /// </summary>
        /// <param name="inData">字节数组</param>
        /// <param name="offset">从哪里开始转换.</param>
        /// <param name="len">转换长度, 如果len超过8则忽略后面的.</param>
        /// <returns></returns>
        public static uint GetUInt(byte[] inData, int offset, int len)
        {
            uint ret = 0;
            int end = 0;
            if (len > 8)
                end = offset + 8;
            else
                end = offset + len;
            for (int i = 0; i < end; i++)
            {
                ret <<= 8;
                ret |= (uint)inData[i];
            }
            return ret;
        }
        public static string LongToHexString(long num)
        {
            return string.Format("{0:X8}", num);
        }
        /// <summary>
        /// 字符串转byte[]数组
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
            byte[] array = new byte[hexString.Length / 2];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
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
        public static string QQToHexString(long qq)
        {
            string text = Convert.ToString(qq, 16);
            if (text.Length == 8)
            {
                return text;
            }
            if (text.Length > 8)
            {
                return null;
            }
            int num = 8 - text.Length;
            string str = "";
            for (int i = 0; i < num; i++)
            {
                str += "0";
            }
            text = (str + text).ToUpper();
            StringBuilder stringBuilder = new StringBuilder();
            for (int j = 0; j < text.Length; j++)
            {
                stringBuilder.Append(text[j]);
                if ((j + 1) % 2 == 0)
                {
                    stringBuilder.Append(" ");
                }
            }
            return stringBuilder.ToString();
        }

        public static string ConvertHexToString(string HexValue)
        {
            byte[] bytes = HexStringToByteArray(HexValue);
            return Encoding.UTF8.GetString(bytes);
        }
        /// <summary>
        /// 根据某种编码方式将字节数组转换成字符串
        /// 
        /// </summary>
        /// <param name="b">字节数组</param>
        /// <param name="encoding">encoding 编码方式</param>
        /// <returns> 如果encoding不支持，返回一个缺省编码的字符串</returns>
        public static string GetString(byte[] b, string encoding)
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
        /// 根据缺省编码将字节数组转换成字符串
        /// 
        /// </summary>
        /// <param name="b">字节数组</param>
        /// <returns>字符串</returns>
        public static string GetString(byte[] b)
        {
            return GetString(b, QQGlobal.QQ_CHARSET_DEFAULT);
        }
        /// <summary>
        /// * 从buf的当前位置解析出一个字符串，直到碰到了buf的结尾
        /// * <p>
        /// * 此方法不负责调整buf位置，调用之前务必使buf当前位置处于字符串开头。在读取完成
        /// * 后，buf当前位置将位于buf最后之后
        /// * </p>
        /// * <p>
        /// * 返回的字符串将使用QQ缺省编码，一般来说就是GBK编码
        /// 	<remark>2008-02-22 </remark>
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <returns></returns>
        public static string GetString(ByteBuffer buf)
        {
            ByteBuffer temp = new ByteBuffer();
            while (buf.HasRemaining())
            {
                temp.Put(buf.Get());
            }
            return GetString(temp.ToByteArray());
        }
        /// <summary>从buf的当前位置解析出一个字符串，直到碰到了buf的结尾或者读取了len个byte之后停止
        /// 此方法不负责调整buf位置，调用之前务必使buf当前位置处于字符串开头。在读取完成
        /// * 后，buf当前位置将位于len字节之后或者最后之后
        /// 	<remark>2008-02-25 </remark>
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static string GetString(ByteBuffer buf, int len)
        {
            ByteBuffer temp = new ByteBuffer();
            while (buf.HasRemaining() && len-- > 0)
            {
                temp.Put(buf.Get());
            }
            return GetString(temp.ToByteArray());
        }

        /// <summary>
        /// * 从buf的当前位置解析出一个字符串，直到碰到了delimit或者读取了maxLen个byte或者
        /// * 碰到结尾之后停止
        /// *此方法不负责调整buf位置，调用之前务必使buf当前位置处于字符串开头。在读取完成
        /// *后，buf当前位置将位于maxLen之后
        /// 	<remark>2008-02-22 </remark>
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="delimit">The delimit.</param>
        /// <param name="maxLen">The max len.</param>
        /// <returns></returns>
        public static String GetString(ByteBuffer buf, byte delimit, int maxLen)
        {
            ByteBuffer temp = new ByteBuffer();
            while (buf.HasRemaining() && maxLen-- > 0)
            {
                byte b = buf.Get();
                if (b == delimit)
                    break;
                else
                    temp.Put(b);
            }
            while (buf.HasRemaining() && maxLen-- > 0)
                buf.Get();
            return GetString(temp.ToByteArray());
        }
        /// <summary>根据某种编码方式将字节数组转换成字符串
        /// 	<remark>2008-02-22 </remark>
        /// </summary>
        /// <param name="b">The b.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="len">The len.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static string GetString(byte[] b, int offset, int len)
        {
            byte[] temp = new byte[len];
            Array.Copy(b, offset, temp, 0, len);
            return GetString(temp);
        }

        /// <summary>
        /// 从buf的当前位置解析出一个字符串，直到碰到一个分隔符为止，或者到了buf的结尾
        /// 此方法不负责调整buf位置，调用之前务必使buf当前位置处于字符串开头。在读取完成
        /// * 后，buf当前位置将位于分隔符之后
        /// 	<remark>2008-02-23 </remark>
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="delimit">The delimit.</param>
        /// <returns></returns>
        public static string GetString(ByteBuffer buf, byte delimit)
        {
            ByteBuffer temp = new ByteBuffer();
            while (buf.HasRemaining())
            {
                byte b = buf.Get();
                if (b == delimit)
                    return GetString(temp.ToByteArray());
                else
                    buf.Put(b);
            }
            return GetString(temp.ToByteArray());
        }

        /// <summary>
        /// 把字符串转换成int
        /// 
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="defaultValue">如果转换失败，返回这个值</param>
        /// <returns></returns>
        public static int GetInt(string s, int defaultValue)
        {
            int value;
            if (int.TryParse(s, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 字符串转二进制字数组
        /// 
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static byte[] GetBytes(string s)
        {
            return DefaultEncoding.GetBytes(s);
        }

        /// <summary>一个随机产生的密钥字节数组
        /// 
        /// </summary>
        /// <returns></returns>
        public static byte[] RandomKey()
        {
            byte[] key = new byte[QQGlobal.QQ_LENGTH_KEY];
            (new Random()).NextBytes(key);
            return key;
        }
        /// <summary>一个随机产生的密钥字节数组
        /// 
        /// </summary>
        /// <returns></returns>
        public static byte[] RandomKey(int length)
        {
            byte[] key = new byte[length];
            (new Random()).NextBytes(key);
            return key;
        }


        /// <summary>
        /// 用于代替 System.currentTimeMillis()
        /// 	<remark>2008-02-29 </remark>
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static long GetTimeMillis(DateTime dateTime)
        {
            return (long)(dateTime - baseDateTime).TotalMilliseconds;
        }
        public static long GetTimeSeconds(DateTime dateTime)
        {
            return (long)(dateTime - baseDateTime).TotalSeconds;
        }
        /// <summary>
        /// 根据服务器返回的毫秒表示的日期，获得实际的日期
        /// Gets the date time from millis.
        /// 似乎服务器返回的日期要加上8个小时才能得到正确的 +8 时区的登录时间
        /// </summary>
        /// <param name="millis">The millis.</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromMillis(long millis)
        {
            return baseDateTime.AddTicks(millis * TimeSpan.TicksPerMillisecond).AddHours(8);
        }
        /// <summary>判断IP是否全0
        /// 
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <returns></returns>
        public static bool IsIPZero(byte[] ip)
        {
            for (int i = 0; i < ip.Length; i++)
            {
                if (ip[i] != 0)
                    return false;
            }
            return true;
        }
        /// <summary>ip的字节数组形式转为字符串形式的ip
        /// 
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <returns></returns>
        public static String GetIpStringFromBytes(byte[] ip)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ip[0] & 0xFF);
            sb.Append('.');
            sb.Append(ip[1] & 0xFF);
            sb.Append('.');
            sb.Append(ip[2] & 0xFF);
            sb.Append('.');
            sb.Append(ip[3] & 0xFF);
            return sb.ToString();
        }
        public static string ToHex(byte[] bs, string NewLine = "", string Format = "{0} ")
        {
            int num = 0;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bs.Length; i++)
            {
                byte b = bs[i];
                if (num++ % 16 == 0)
                {
                    stringBuilder.Append(NewLine);
                }
                stringBuilder.AppendFormat(Format, b.ToString("X2"));
            }
            return stringBuilder.ToString().Trim();
        }
        public static byte[] IPStringToByteArray(string ip)
        {
            byte[] array = new byte[4];
            string[] array2 = ip.Split(new char[]
            {
                '.'
            });
            if (array2.Length == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    array[i] = (byte)int.Parse(array2[i]);
                }
            }
            return array;
        }

        /// <summary>
        /// 获取本地外网 IP
        /// </summary>
        /// <returns></returns>
        public static string GetExternalIp()
        {
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string response = client.DownloadString("http://www.baidu.com/s?wd=ip&rsv_spt=1&rsv_iqid=0xa699f31100003c46&issp=1&f=3&rsv_bp=0&rsv_idx=2&ie=utf-8&tn=baiduhome_pg&rsv_enter=0&rsv_sug3=2&rsv_sug1=1&rsv_sug7=100&prefixsug=ip&rsp=1&inputT=1711&rsv_sug4=1711");//百度
            string myReg = @"<span class=""c-gap-right"">([\s\S]+?)<\/span>";
            Match mc = Regex.Match(response, myReg, RegexOptions.Singleline);
            if (mc.Success && mc.Groups.Count > 1)
            {
                response = mc.Groups[1].Value.Replace("本机IP:&nbsp;", "");
                return response;
            }
            else
            {
                throw new Exception("获取IP失败");
            }

        }
        /// <summary>
        /// 根据域名获取IP
        /// </summary>
        /// <param name="hostname">域名</param>
        /// <returns></returns>
        public static string GetHostAddresses(string hostname)
        {
            IPAddress[] ips;

            ips = Dns.GetHostAddresses(hostname);

            return ips[0].ToString();
        }

        public static string GET_GTK(string str)
        {
            var hash = 5381;
            var hashstr = hash.ToString();
            for (int i = 0, len = str.Length; i < len; ++i)
            {
                hash += (hash << 5) + (int)str[i];
                hashstr = "((" + hashstr + "<<5)+" + ((int)str[i]).ToString() + ")";
            }
            return (hash & 0x7fffffff).ToString();
        }

        /// <summary>
        /// 一个特殊的加密
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
                    binary = binary.Substring(0, binary.Length - 7);
                else
                    break;
            }
            var temp1 = temp.Substring(temp.Length - 7, 7);
            temp = temp.Substring(0, temp.Length - 8) + "0" + temp1;
            return Util.LongToHexString(Convert.ToInt64(temp, 2));
        }
        #region TLV专属操作方法
        public static void int16_to_buf(byte[] TEMP_BYTE_ARRAY, int index, int Num)
        {
            TEMP_BYTE_ARRAY[index] = (byte)(((Num & 0xff000000) >> 24) & 0xff);
            TEMP_BYTE_ARRAY[index + 1] = (byte)(((Num & 0x00ff0000) >> 16) & 0xff);
            TEMP_BYTE_ARRAY[index + 2] = (byte)(((Num & 0x0000ff00) >> 8) & 0xff);
            TEMP_BYTE_ARRAY[index + 3] = (byte)((Num & 0x000000ff) & 0xff);
        }
        public static int buf_to_int16(byte[] TEMP_BYTE_ARRAY, int index)
        {
            return TEMP_BYTE_ARRAY[index] << 24 | TEMP_BYTE_ARRAY[index + 1] << 16 | TEMP_BYTE_ARRAY[index + 2] << 8 | TEMP_BYTE_ARRAY[index + 3];
        }

        public static long currentTimeMillis()
        {
           return GetTimeMillis(DateTime.Now);
        }

        internal static string MapPath(string directory)
        {
            return AppDomain.CurrentDomain.BaseDirectory + "//" + directory;
        }
        #endregion
    }
}
