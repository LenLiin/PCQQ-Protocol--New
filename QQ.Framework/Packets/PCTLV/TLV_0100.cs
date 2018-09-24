using System;
using System.IO;
using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.ErrorCode)]
    internal class TLV_0100 : BaseTLV
    {
        public TLV_0100()
        {
            cmd = 0x0100;
            Name = "SSO2::TLV_ErrorCode_0x100";
        }

        public string ErrorMsg { get; private set; }
        public char PacketCommand { get; private set; }

        public void Parser_Tlv(QQUser User, BinaryReader buf)
        {
            var _type = buf.BEReadUInt16();//type
            var _length = buf.BEReadUInt16();//length
            Parser_Tlv2(User, buf, _length);
        }
        public void Parser_Tlv2(QQUser User, BinaryReader buf,int Length)
        {
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                PacketCommand = (char)buf.BEReadUInt16();
                var ErrorCode = buf.BEReadUInt32();
                ErrorMsg = Encoding.UTF8.GetString(buf.ReadBytes(buf.BEReadUInt16()));
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}