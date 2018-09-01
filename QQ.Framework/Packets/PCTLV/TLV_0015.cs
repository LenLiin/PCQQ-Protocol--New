using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0015 : BaseTLV
    {
        public TLV_0015()
        {
            this.cmd = 0x0015;
            this.Name = "SSO2::TLV_ComputerGuid_0x15";
            this.wSubVer = 0x0001;
        }

        public byte[] get_tlv_0015(QQClient m_PCClient)
        {
              var data = new BinaryWriter(new MemoryStream());
            if (this.wSubVer == 0x0001)
            {
                data.BEWrite(this.wSubVer); //wSubVer

                data.Write(0x01);
                byte[] thisKey = m_PCClient.QQUser.TXProtocol.bufComputerID;
                data.BEWrite(CRC32cs.CRC32(thisKey));
                data.Write(thisKey);

                data.Write(0x02);
                thisKey = m_PCClient.QQUser.TXProtocol.bufComputerIDEx;
                data.BEWrite(CRC32cs.CRC32(thisKey));
                data.Write(thisKey);
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
