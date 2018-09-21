using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags._0x0033)]
    internal class TLV_0033 : BaseTLV
    {
        public TLV_0033()
        {
            cmd = 0x0033;
            Name = "SSO2::TLV_LoginReason_0x33";
            wSubVer = 0x0002;
        }
        public void Parser_Tlv(QQUser User, BinaryReader buf)
        {
            var _type = buf.BEReadUInt16();//type
            var _length = buf.BEReadUInt16();//length
            buf.ReadBytes(_length);
        }
    }
}