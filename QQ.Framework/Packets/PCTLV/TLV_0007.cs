using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.TGT)]
    internal class TLV_0007 : BaseTLV
    {
        public TLV_0007()
        {
            cmd = 0x0007;
            Name = "TLV_TGT";
        }

        public byte[] Get_Tlv(QQUser User)
        {
            var bufTGT = User.TXProtocol.bufTGT;
            var buf = new BinaryWriter(new MemoryStream());
            buf.Write(bufTGT);
            fill_head(cmd);
            fill_body(buf.BaseStream.ToBytesArray(), buf.BaseStream.Length);
            set_length();
            return get_buf();
        }
    }
}