using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    internal class TLV_0110 : BaseTLV
    {
        public TLV_0110()
        {
            cmd = 0x0110;
            Name = "SSO2::TLV_SigPic_0x110";
            wSubVer = 0x0001;
        }

        public byte[] get_tlv_0110(QQClient m_PCClient)
        {
            if (m_PCClient.QQUser.TXProtocol.bufSigPic == null)
            {
                return null;
            }

            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0001)
            {
                data.BEWrite(wSubVer); //wSubVer
                data.Write(m_PCClient.QQUser.TXProtocol.bufSigPic);
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

        public void parser_tlv_0110(QQClient m_PCClient, BinaryReader buf)
        {
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                m_PCClient.QQUser.TXProtocol.bufSigPic = buf.ReadBytes(0x38);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}