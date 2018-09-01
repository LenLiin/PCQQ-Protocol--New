using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;
using System.Text;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_000A : BaseTLV
    {
        public TLV_000A()
        {
            this.cmd = 0x000A;
            this.Name = "SSO2::TLV_ErrorCode_0x000A";
        }

        public void parser_tlv_0A(QQClient m_PCClient, BinaryReader buf)
        {
            this.wSubVer = buf.BEReadUInt16(); //wSubVer
            if (this.wSubVer == 0x0001)
            {
                var wCsCmd = buf.BEReadUInt16();
                this.ErrorMsg = Encoding.UTF8.GetString(buf.ReadBytes(wCsCmd));
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
        }

        public string ErrorMsg { get; private set; }
    }
}
