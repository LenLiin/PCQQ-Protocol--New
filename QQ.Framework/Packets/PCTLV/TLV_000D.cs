using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags._0x000D)]
    internal class TLV000D : BaseTLV
    {
        public TLV000D()
        {
            Command = 0x000D;
            Name = "TLV_000D";
        }

        public void Parser_Tlv(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            WSubVer = buf.BeReadUInt16(); //wSubVer
            if (WSubVer == 0x0001)
            {
                buf.BeReadInt32(); //UNKNOW
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }
        }
    }
}