using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    internal class TLV_0104 : BaseTLV
    {
        public TLV_0104()
        {
            cmd = 0x0104;
            Name = "TLV_0104";
        }

        public byte PngData { get; private set; }

        /// <summary>
        ///     是否还有验证码数据
        /// </summary>
        public byte Next { get; private set; }

        public void parser_tlv_0006(QQClient m_PCClient, BinaryReader buf)
        {
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                var wCsCmd = buf.BEReadUInt16();
                var ErrorCode = buf.BEReadUInt32();

                buf.ReadByte(); //0x00
                buf.ReadByte(); //0x05
                PngData = buf.ReadByte(); //是否需要验证码：0不需要，1需要
                int len;
                if (PngData == 0x00)
                {
                    len = buf.ReadByte();
                    while (len == 0)
                    {
                        len = buf.ReadByte();
                    }
                }
                else //ReplyCode != 0x01按下面走 兼容多版本
                {
                    buf.BEReadInt32(); //需要验证码时为00 00 01 23，不需要时为全0
                    len = buf.BEReadInt32();
                }

                var buffer = buf.ReadBytes(len);
                m_PCClient.QQUser.TXProtocol.bufSigPic = buffer;
                if (PngData == 0x01) //有验证码数据
                {
                    len = buf.BEReadInt32();
                    buffer = buf.ReadBytes(len);
                    Next = buf.ReadByte();
                    buf.ReadByte();
                    var directory = Util.MapPath("Verify");
                    var filename = Path.Combine(directory, m_PCClient.QQUser.QQ + ".png");
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    var fs = Next == 0x00
                        ? new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read)
                        : new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.Read);

                    //fs.Seek(0, SeekOrigin.End);
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();

                    len = buf.BEReadInt32();
                    buffer = buf.ReadBytes(len);
                    m_PCClient.QQUser.TXProtocol.PngToken = buffer;

                    buf.BEReadUInt16();

                    len = buf.BEReadInt32();
                    buffer = buf.ReadBytes(len);
                    m_PCClient.QQUser.TXProtocol.PngKey = buffer;
                }
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}