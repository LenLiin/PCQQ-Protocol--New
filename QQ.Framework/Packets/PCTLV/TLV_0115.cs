using System.IO;
using QQ.Framework;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0115 : BaseTLV
    {
        public TLV_0115()
        {
            cmd = 0x0115;
            Name = "SSO2::TLV_PacketMd5_0x115";
        }

        public void parser_tlv_0115(QQClient m_PCClient, BinaryReader buf)
        {
            var bufPacketMD5 = buf.ReadBytes(0x38);
        }
    }
}