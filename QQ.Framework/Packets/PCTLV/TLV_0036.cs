using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0036 : BaseTLV
    {
        public TLV_0036()
        {
            this.cmd = 0x0036;
            this.Name = "SSO2::TLV_LoginReason_0x36";
            this.wSubVer = 0x0002;
        }

        public byte[] get_tlv_0036(QQClient m_PCClient)
        {
              var data = new BinaryWriter(new MemoryStream());
            if (this.wSubVer == 0x0002)
            {
                data.BEWrite(this.wSubVer); //wSubVer
                data.BEWrite(1);
                data.BEWrite(0);
                data.BEWrite(0);
                data.BEWrite(0);
                data.BEWrite(0);
                data.Write(0);
                data.Write(0);
            }
            else if (this.wSubVer == 0x0001)
            {
                data.BEWrite(this.wSubVer); //wSubVer
                data.BEWrite(1);
                data.BEWrite(0);
                data.BEWrite(0);
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
            fill_head(this.cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }
    }
}
