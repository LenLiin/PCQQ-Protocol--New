using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0112 : BaseTLV
    {
        public TLV_0112()
        {
            this.cmd = 0x0112;
            this.Name = "SSO2::TLV_SigIP2_0x112";
        }

        public byte[] get_tlv_0112(QQClient m_PCClient)
        {
            var data = new BinaryWriter(new MemoryStream());
            data.Write( m_PCClient.QQUser.TXProtocol.bufSigClientAddr);
            fill_head(this.cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }

        public void parser_tlv_0112(QQClient m_PCClient, BinaryReader buf)
        {
             m_PCClient.QQUser.TXProtocol.bufSigClientAddr = buf.ReadBytes((int)(buf.BaseStream.Length- buf.BaseStream.Position));
        }
    }
}
