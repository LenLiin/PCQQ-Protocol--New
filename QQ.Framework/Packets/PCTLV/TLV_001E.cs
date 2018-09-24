using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.GTKey_TGTGT)]
    internal class TLV001E : BaseTLV
    {
        public TLV001E()
        {
            Command = 0x001E;
            Name = "SSO2::TLV_GTKey_TGTGT_0x1e";
        }

        public void Parser_Tlv(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            user.TXProtocol.BufTgtgtKey =
                buf.ReadBytes(length);
        }
    }
}