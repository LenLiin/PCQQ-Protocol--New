using System;
using System.IO;
using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.ErrorCode)]
    internal class TLV0100 : BaseTLV
    {
        public TLV0100()
        {
            Command = 0x0100;
            Name = "SSO2::TLV_ErrorCode_0x100";
        }

        public string ErrorMsg { get; private set; }
        public char PacketCommand { get; private set; }

        public void Parser_Tlv(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            Parser_Tlv2(user, buf, length);
        }

        public void Parser_Tlv2(QQUser user, BinaryReader buf, int length)
        {
            WSubVer = buf.BeReadUInt16(); //wSubVer
            if (WSubVer == 0x0001)
            {
                PacketCommand = (char) buf.BeReadUInt16();
                var errorCode = buf.BeReadUInt32();
                ErrorMsg = Encoding.UTF8.GetString(buf.ReadBytes(buf.BeReadUInt16()));
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }
        }
    }
}