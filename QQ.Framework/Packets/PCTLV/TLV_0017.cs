using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    internal class TLV_0017 : BaseTLV
    {
        public TLV_0017()
        {
            cmd = 0x0017;
            Name = "SSO2::TLV_ClientInfo_0x17";
        }

        public void parser_tlv_0017(QQClient m_PCClient, BinaryReader buf)
        {
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                long TimeMillis = buf.BEReadUInt32();
                m_PCClient.dwServerTime = Util.GetDateTimeFromMillis(TimeMillis);
                QQGlobal.time_difference = (uint.MaxValue & TimeMillis) - Util.currentTimeMillis() / 1000;
                m_PCClient.dwClientIP = Util.GetIpStringFromBytes(buf.ReadBytes(4));
                m_PCClient.wClientPort = buf.BEReadUInt16();
                buf.BEReadUInt16(); //UNKNOW
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}