using System;
using System.IO;
using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.ErrorInfo)]
    internal class TLV_000A : BaseTLV
    {
        public TLV_000A()
        {
            cmd = 0x000A;
            Name = "SSO2::TLV_ErrorCode_0x000A";
        }

        public string ErrorMsg { get; private set; }

        public void Parser_Tlv_0A(QQUser User, BinaryReader buf)
        {
            var _type = buf.BEReadUInt16();//type
            var _length = buf.BEReadUInt16();//length
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                var wCsCmd = buf.BEReadUInt16();
                ErrorMsg = Encoding.UTF8.GetString(buf.ReadBytes(wCsCmd));
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}