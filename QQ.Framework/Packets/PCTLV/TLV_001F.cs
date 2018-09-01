using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_001F : BaseTLV
    {
        public TLV_001F()
        {
            this.cmd = 0x001F;
            this.Name = "TLV_DeviceID";
            this.wSubVer = 0x0001;
        }

        public byte[] get_tlv_001F(QQClient m_PCClient)
        {
            if (m_PCClient.QQUser.TXProtocol.bufDeviceID == null)
            {
                return null;
            }
              var data = new BinaryWriter(new MemoryStream());
            if (this.wSubVer == 0x0001)
            {
                data.BEWrite(this.wSubVer); //wSubVer
                data.Write(m_PCClient.QQUser.TXProtocol.bufDeviceID);
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

        public void parser_tlv_001f(QQClient m_PCClient, BinaryReader buf)
        {
            this.wSubVer = buf.BEReadUInt16(); //wSubVer
            if (this.wSubVer == 0x0001)
            {
                m_PCClient.QQUser.TXProtocol.bufDeviceID = buf.ReadBytes((int)(buf.BaseStream.Length- buf.BaseStream.Position));
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
        }
    }
}
