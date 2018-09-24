using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.ClientInfo)]
    internal class TLV0017 : BaseTLV
    {
        public TLV0017()
        {
            Command = 0x0017;
            Name = "SSO2::TLV_ClientInfo_0x17";
        }

        public void Parser_Tlv(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            WSubVer = buf.BeReadUInt16(); //wSubVer
            if (WSubVer == 0x0001)
            {
                long timeMillis = buf.BeReadUInt32();
                user.TXProtocol.DwServerTime = Util.GetDateTimeFromMillis(timeMillis);
                user.TXProtocol.TimeDifference = (uint.MaxValue & timeMillis) - Util.CurrentTimeMillis() / 1000;
                user.TXProtocol.DwClientIP = Util.GetIpStringFromBytes(buf.ReadBytes(4));
                user.TXProtocol.WClientPort = buf.BeReadUInt16();
                buf.BeReadUInt16(); //UNKNOW
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }
        }
    }
}