using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags._ddReply)]
    internal class TLV_0109 : BaseTLV
    {
        public TLV_0109()
        {
            cmd = 0x0109;
            Name = "SSO2::TLV_0xddReply_0x109";
        }

        public void Parser_Tlv(QQUser User, BinaryReader buf)
        {
            var _type = buf.BEReadUInt16(); //type
            var _length = buf.BEReadUInt16(); //length
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                var buffer = buf.ReadBytes(16);
                User.TXProtocol.bufSessionKey = buffer;

                var len = buf.BEReadUInt16();
                buffer = buf.ReadBytes(len);
                User.TXProtocol.bufSigSession = buffer;

                len = buf.BEReadUInt16();
                buffer = buf.ReadBytes(len);
                User.TXProtocol.bufPwdForConn = buffer;
                buf.BEReadUInt16(); //bufBill
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}