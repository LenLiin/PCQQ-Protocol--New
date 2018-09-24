using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.ClientInfo)]
    internal class TLV_0017 : BaseTLV
    {
        public TLV_0017()
        {
            cmd = 0x0017;
            Name = "SSO2::TLV_ClientInfo_0x17";
        }

        public void Parser_Tlv(QQUser User, BinaryReader buf)
        {
            var _type = buf.BEReadUInt16(); //type
            var _length = buf.BEReadUInt16(); //length
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                long TimeMillis = buf.BEReadUInt32();
                User.TXProtocol.dwServerTime = Util.GetDateTimeFromMillis(TimeMillis);
                User.TXProtocol.time_difference = (uint.MaxValue & TimeMillis) - Util.currentTimeMillis() / 1000;
                User.TXProtocol.dwClientIP = Util.GetIpStringFromBytes(buf.ReadBytes(4));
                User.TXProtocol.wClientPort = buf.BEReadUInt16();
                buf.BEReadUInt16(); //UNKNOW
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}