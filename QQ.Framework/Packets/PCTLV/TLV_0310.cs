using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.ServerAddress)]
    internal class TLV0310 : BaseTLV
    {
        public TLV0310()
        {
            Command = 0x0310;
            Name = "SSO2::TLV_ServerAddress_0x310";
        }

        public void Parser_Tlv(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            user.TXProtocol.DwServerIP = Util.GetIpStringFromBytes(buf.ReadBytes(4));
        }
    }
}