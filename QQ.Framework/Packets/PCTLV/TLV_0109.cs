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
            WSubVer = buf.BeReadUInt16(); //wSubVer
            if (WSubVer == 0x0001)
            {
                var buffer = buf.ReadBytes(16);
                user.TXProtocol.BufSessionKey = buffer;

                var len = buf.BeReadUInt16();
                buffer = buf.ReadBytes(len);
                user.TXProtocol.BufSigSession = buffer;

                len = buf.BeReadUInt16();
                buffer = buf.ReadBytes(len);
                user.TXProtocol.BufPwdForConn = buffer;
                buf.BeReadUInt16(); //bufBill
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }
        }
    }
}