using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0310 : BaseTLV
    {
        public TLV_0310()
        {
            cmd = 0x0310;
            Name = "SSO2::TLV_ServerAddress_0x310";
        }

        public void parser_tlv_0310(QQClient m_PCClient, BinaryReader buf)
        {
            m_PCClient.dwServerIP = Util.GetIpStringFromBytes(buf.ReadBytes(4));
        }
    }
}