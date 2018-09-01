using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_001E : BaseTLV
    {
        public TLV_001E()
        {
            this.cmd = 0x001E;
            this.Name = "SSO2::TLV_GTKey_TGTGT_0x1e";
        }

        public void parser_tlv_001E(QQClient m_PCClient, BinaryReader buf)
        {
            m_PCClient.QQUser.TXProtocol.bufTGTGTKey = buf.ReadBytes((int)(buf.BaseStream.Length- buf.BaseStream.Position));
        }
    }
}
