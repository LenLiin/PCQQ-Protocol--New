using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    internal class TLV_001F : BaseTLV
    {
        public TLV_001F()
        {
            cmd = 0x001F;
            Name = "TLV_DeviceID";
            wSubVer = 0x0001;
        }

        public byte[] get_tlv_001F(QQClient m_PCClient)
        {
            if (m_PCClient.QQUser.TXProtocol.bufDeviceID == null)
            {
                return null;
            }

            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0001)
            {
                data.BEWrite(wSubVer); //wSubVer
                data.Write(m_PCClient.QQUser.TXProtocol.bufDeviceID);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }

            fill_head(cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }

        public void parser_tlv_001f(QQClient m_PCClient, BinaryReader buf)
        {
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                m_PCClient.QQUser.TXProtocol.bufDeviceID =
                    buf.ReadBytes((int) (buf.BaseStream.Length - buf.BaseStream.Position));
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}