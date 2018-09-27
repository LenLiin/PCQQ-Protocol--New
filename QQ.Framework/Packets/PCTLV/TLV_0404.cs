using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags._0x0404)]
    internal class TLV0404 : BaseTLV
    {
        public TLV0404()
        {
            Command = 0x0404;
            Name = "SSO2::TLV_0404";
        }


        public void Parser_Tlv(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            buf.ReadBytes(length);
        }
    }
}