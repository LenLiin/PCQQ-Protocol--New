using System;
using System.IO;
using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.ErrorInfo)]
    internal class TLV000A : BaseTLV
    {
        public TLV000A()
        {
            Command = 0x000A;
            Name = "SSO2::TLV_ErrorCode_0x000A";
        }

        public string ErrorMsg { get; private set; }

        public void Parser_Tlv_0A(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            WSubVer = buf.BeReadUInt16(); //wSubVer
            if (WSubVer == 0x0001)
            {
                var wCsCmd = buf.BeReadUInt16();
                ErrorMsg = Encoding.UTF8.GetString(buf.ReadBytes(wCsCmd));
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }
        }
    }
}