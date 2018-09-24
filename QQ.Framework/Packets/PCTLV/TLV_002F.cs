using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags._0x002F)]
    internal class TLV_002F : BaseTLV
    {
        public TLV_002F()
        {
            cmd = 0x002F;
            Name = "TLV_Control";
        }

        public void Parser_Tlv(QQUser User, BinaryReader buf)
        {
            var _type = buf.BEReadUInt16(); //type
            var _length = buf.BEReadUInt16(); //length
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                var bufControl = buf.ReadBytes(_length - 2);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}