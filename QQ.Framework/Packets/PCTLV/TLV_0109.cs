using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags._ddReply)]
    internal class TLV0109 : BaseTLV
    {
        public TLV0109()
        {
            Command = 0x0109;
            Name = "SSO2::TLV_0xddReply_0x109";
        }

        public void Parser_Tlv(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            var Data = buf.ReadBytes(length);
            var bufData = new BinaryReader(new MemoryStream(Data));
            WSubVer = bufData.BeReadUInt16(); //wSubVer
            if (WSubVer == 0x0001)
            {
                var buffer = bufData.ReadBytes(16);
                user.TXProtocol.BufSessionKey = buffer;

                var len = bufData.BeReadUInt16();
                buffer = bufData.ReadBytes(len);
                user.TXProtocol.BufSigSession = buffer;

                len = bufData.BeReadUInt16();
                buffer = bufData.ReadBytes(len);
                user.TXProtocol.BufPwdForConn = buffer;
                if (bufData.BaseStream.Length > bufData.BaseStream.Position)
                {
                    len = bufData.BeReadUInt16(); //bufBill
                    if (len > 0)
                    {
                        bufData.ReadBytes(len);
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