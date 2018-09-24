using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.SigLastLoginInfo)]
    internal class TLV_010D : BaseTLV
    {
        public TLV_010D()
        {
            cmd = 0x010D;
            Name = "TLV_SigLastLoginInfo";
        }

        public void Parser_Tlv(QQUser User, BinaryReader buf)
        {
            var _type = buf.BEReadUInt16(); //type
            var _length = buf.BEReadUInt16(); //length
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                var bufSigLastLoginInfo = buf.ReadBytes(_length - 2);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}