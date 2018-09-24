using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags._0x0033)]
    internal class TLV0033 : BaseTLV
    {
        public TLV0033()
        {
            Command = 0x0033;
            Name = "SSO2::TLV_LoginReason_0x33";
            WSubVer = 0x0002;
        }

        public void Parser_Tlv(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            buf.ReadBytes(length);
        }
    }
}