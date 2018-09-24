using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.TGT)]
    internal class TLV0007 : BaseTLV
    {
        public TLV0007()
        {
            Command = 0x0007;
            Name = "TLV_TGT";
        }

        public byte[] Get_Tlv(QQUser user)
        {
            var bufTgt = user.TXProtocol.BufTgt;
            var buf = new BinaryWriter(new MemoryStream());
            buf.Write(bufTgt);
            FillHead(Command);
            FillBody(buf.BaseStream.ToBytesArray(), buf.BaseStream.Length);
            SetLength();
            return GetBuffer();
        }
    }
}