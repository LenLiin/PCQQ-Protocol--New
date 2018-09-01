using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;
using System.Text;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0004 : BaseTLV
    {
        public TLV_0004()
        {
            this.cmd = 0x0004;
            this.Name = "SSO2::TLV_NonUinAccount_0x4";
            this.wSubVer = 0x0000;
        }

        public byte[] get_tlv_0004(QQClient m_PCClient)
        {
            if (m_PCClient.QQUser.QQ != 0)
            {
                return null;
            }
            var data = new BinaryWriter(new MemoryStream());
            if (this.wSubVer == 0x0000)
            {
                data.BEWrite(this.wSubVer); //wSubVer 
                var bufAccount = Util.HexStringToByteArray(Util.QQToHexString(m_PCClient.QQUser.QQ));
                data.BEWrite((ushort)bufAccount.Length);//账号长度
                data.Write(bufAccount);//账号
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
