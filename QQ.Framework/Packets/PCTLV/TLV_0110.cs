using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0110 : BaseTLV
    {
        public TLV_0110()
        {
            this.cmd = 0x0110;
            this.Name = "SSO2::TLV_SigPic_0x110";
            this.wSubVer = 0x0001;
        }

        public byte[] get_tlv_0110(QQClient m_PCClient)
        {
            if ( m_PCClient.QQUser.TXProtocol.bufSigPic == null)
            {
                return null;
            }
              var data = new BinaryWriter(new MemoryStream());
            if (this.wSubVer == 0x0001)
            {
                data.BEWrite(this.wSubVer); //wSubVer
                data.Write( m_PCClient.QQUser.TXProtocol.bufSigPic);
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

        public void parser_tlv_0110(QQClient m_PCClient, BinaryReader buf)
        {
            this.wSubVer = buf.BEReadUInt16(); //wSubVer
            if (this.wSubVer == 0x0001)
            {
                 m_PCClient.QQUser.TXProtocol.bufSigPic = buf.ReadBytes(0x38);
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
        }
    }
}
