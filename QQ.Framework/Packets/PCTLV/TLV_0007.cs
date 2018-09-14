using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    internal class TLV_0007 : BaseTLV
    {
        public TLV_0007()
        {
            cmd = 0x0007;
            Name = "TLV_TGT";
        }

        public byte[] get_tlv_0007(QQClient m_PCClient)
        {
            var bufTGT = m_PCClient.QQUser.TXProtocol.bufTGT;
            var buf = new BinaryWriter(new MemoryStream());
            buf.Write(bufTGT);
            fill_head(cmd);
            fill_body(buf.BaseStream.ToBytesArray(), buf.BaseStream.Length);
            set_length();
            return get_buf();
        }
    }
}