using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.ServerAddress)]
    internal class TLV_0310 : BaseTLV
    {
        public TLV_0310()
        {
            cmd = 0x0310;
            Name = "SSO2::TLV_ServerAddress_0x310";
        }

        public void Parser_Tlv(QQUser User, BinaryReader buf)
        {
            var _type = buf.BEReadUInt16();//type
            var _length = buf.BEReadUInt16();//length
            User.TXProtocol.dwServerIP = Util.GetIpStringFromBytes(buf.ReadBytes(4));
        }
    }
}