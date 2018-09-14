using System.IO;
using QQ.Framework;

namespace QQ.Framework.Packets.PCTLV
{
    internal class TLV_001E : BaseTLV
    {
        public TLV_001E()
        {
            cmd = 0x001E;
            Name = "SSO2::TLV_GTKey_TGTGT_0x1e";
        }

        public void parser_tlv_001E(QQClient m_PCClient, BinaryReader buf)
        {
            m_PCClient.QQUser.TXProtocol.bufTGTGTKey =
                buf.ReadBytes((int) (buf.BaseStream.Length - buf.BaseStream.Position));
        }
    }
}