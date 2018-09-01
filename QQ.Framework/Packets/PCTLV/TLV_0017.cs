using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0017 : BaseTLV
    {
        public TLV_0017()
        {
            this.cmd = 0x0017;
            this.Name = "SSO2::TLV_ClientInfo_0x17";
        }

        public void parser_tlv_0017(QQClient m_PCClient, BinaryReader buf)
        {
            this.wSubVer = buf.BEReadUInt16(); //wSubVer
            if (this.wSubVer == 0x0001)
            {
                long TimeMillis = buf.BEReadUInt32();
                m_PCClient.dwServerTime = Util.GetDateTimeFromMillis(TimeMillis);
                QQGlobal.time_difference = (uint.MaxValue & TimeMillis) - Util.currentTimeMillis() / 1000;
                m_PCClient.dwClientIP = Util.GetIpStringFromBytes(buf.ReadBytes(4));
                m_PCClient.wClientPort = buf.BEReadUInt16();
                buf.BEReadUInt16();//UNKNOW
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
        }
    }
}
