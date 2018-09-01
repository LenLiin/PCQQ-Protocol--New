using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0115 : BaseTLV
    {
        public TLV_0115()
        {
            this.cmd = 0x0115;
            this.Name = "SSO2::TLV_PacketMd5_0x115";
        }

        public void parser_tlv_0115(QQClient m_PCClient, BinaryReader buf)
        {
            var bufPacketMD5 = buf.ReadBytes(0x38);
        }
    }
}
