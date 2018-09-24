using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.SigLastLoginInfo)]
    internal class TLV010D : BaseTLV
    {
        public TLV010D()
        {
            Command = 0x010D;
            Name = "TLV_SigLastLoginInfo";
        }

        public void Parser_Tlv(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            WSubVer = buf.BeReadUInt16(); //wSubVer
            if (WSubVer == 0x0001)
            {
                var bufSigLastLoginInfo = buf.ReadBytes(length - 2);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }
        }
    }
}