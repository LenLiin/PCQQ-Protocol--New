using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.GTKey_TGTGT)]
    internal class TLV_001E : BaseTLV
    {
        public TLV_001E()
        {
            cmd = 0x001E;
            Name = "SSO2::TLV_GTKey_TGTGT_0x1e";
        }

        public void Parser_Tlv(QQUser User, BinaryReader buf)
        {
            var _type = buf.BEReadUInt16();//type
            var _length = buf.BEReadUInt16();//length
            User.TXProtocol.bufTGTGTKey =
                buf.ReadBytes(_length);
        }
    }
}