using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0005 : BaseTLV
    {
        public TLV_0005()
        {
            this.cmd = 0x0005;
            this.Name = "SSO2::TLV_Uin_0x5";
            this.wSubVer = 0x0002;
        }
        public byte[] get_tlv_0005(QQClient m_PCClient)
        {
            if (m_PCClient.QQUser.QQ == 0)
            {
                return null;
            }
            var buf = new BinaryWriter(new MemoryStream());
            if (this.wSubVer == 0x0002)
            {
                buf.BEWrite((ushort)this.wSubVer);
                buf.BEWrite((uint)m_PCClient.QQUser.QQ);
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
            fill_head(this.cmd);
            fill_body(buf.BaseStream.ToBytesArray(), buf.BaseStream.Length);
            set_length();
            return get_buf();
        }
    }
}
