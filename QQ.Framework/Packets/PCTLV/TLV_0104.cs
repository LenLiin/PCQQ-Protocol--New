using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags._0x0104)]
    internal class TLV0104 : BaseTLV
    {
        public TLV0104()
        {
            Command = 0x0104;
            Name = "TLV_0104";
        }

        public byte PngData { get; private set; }

        /// <summary>
        ///     是否还有验证码数据
        /// </summary>
        public byte Next { get; private set; }

        public void Parser_Tlv(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            var Data = buf.ReadBytes(length);
            var bufData = new BinaryReader(new MemoryStream(Data));
            WSubVer = bufData.BeReadUInt16(); //wSubVer
            if (WSubVer == 0x0001)
            {
                var wCsCmd = bufData.BeReadUInt16();
                var errorCode = bufData.BeReadUInt32();

                bufData.ReadByte(); //0x00
                bufData.ReadByte(); //0x05
                PngData = bufData.ReadByte(); //是否需要验证码：0不需要，1需要
                int len;
                if (PngData == 0x00)
                {
                    len = bufData.ReadByte();
                    while (len == 0)
                    {
                        len = bufData.ReadByte();
                    }
                }
                else //ReplyCode != 0x01按下面走 兼容多版本
                {
                    bufData.BeReadInt32(); //需要验证码时为00 00 01 23，不需要时为全0
                    len = bufData.BeReadUInt16();
                }

                var buffer = bufData.ReadBytes(len);
                user.TXProtocol.BufSigPic = buffer;
                if (PngData == 0x01) //有验证码数据
                {
                    len = bufData.BeReadUInt16();
                    buffer = bufData.ReadBytes(len);
                    user.QQPacket00BaVerifyCode = buffer;
                    Next = bufData.ReadByte();
                    bufData.ReadByte();
                    //var directory = Util.MapPath("Verify");
                    //var filename = Path.Combine(directory, user.QQ + ".png");
                    //if (!Directory.Exists(directory))
                    //{
                    //    Directory.CreateDirectory(directory);
                    //}

                    //var fs = Next == 0x00
                    //    ? new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read)
                    //    : new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.Read);

                    ////fs.Seek(0, SeekOrigin.End);
                    //fs.Write(buffer, 0, buffer.Length);
                    //fs.Close();
                    len = bufData.BeReadUInt16();
                    buffer = bufData.ReadBytes(len);
                    user.TXProtocol.PngToken = buffer;
                    if (bufData.BaseStream.Length > bufData.BaseStream.Position)
                    {
                        bufData.BeReadUInt16();
                        len = bufData.BeReadUInt16();
                        buffer = bufData.ReadBytes(len);
                        user.TXProtocol.PngKey = buffer;
                    }
                }
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }
        }
    }
}