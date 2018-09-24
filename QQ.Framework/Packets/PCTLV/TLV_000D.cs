using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags._0x000D)]
    internal class TLV_000D : BaseTLV
    {
        public TLV_000D()
        {
            cmd = 0x000D;
            Name = "TLV_000D";
        }

        public void Parser_Tlv(QQUser User, BinaryReader buf)
        {
            var _type = buf.BEReadUInt16(); //type
            var _length = buf.BEReadUInt16(); //length
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                buf.BEReadInt32(); //UNKNOW
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}