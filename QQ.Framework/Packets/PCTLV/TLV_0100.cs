using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.Text;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0100 : BaseTLV
    {
        public TLV_0100()
        {
            this.cmd = 0x0100;
            this.Name = "SSO2::TLV_ErrorCode_0x100";
        }

        public void parser_tlv_0006(QQClient m_PCClient, BinaryReader buf)
        {
            this.wSubVer = buf.BEReadUInt16(); //wSubVer
            if (this.wSubVer == 0x0001)
            {
                var wCsCmd = buf.BEReadUInt16();
                var ErrorCode = buf.BEReadUInt32();
                this.ErrorMsg = Encoding.UTF8.GetString(buf.ReadBytes(0x38));
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
        }

        public string ErrorMsg { get; private set; }
    }
}
