using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0114 : BaseTLV
    {
        public TLV_0114()
        {
            cmd = 0x0114;
            Name = "SSO2::TLV_DHParams_0x114";
            wSubVer = 0x0102;
        }

        public byte[] get_tlv_0114(QQClient m_PCClient)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0102)
            {
                data.BEWrite(wSubVer); //wDHVer
                data.BEWrite((ushort) m_PCClient.QQUser.TXProtocol.bufDHPublicKey.Length); //bufDHPublicKey长度
                data.Write(m_PCClient.QQUser.TXProtocol.bufDHPublicKey);
                //client.TXProtocol.Key["DHDecodeKey"] = client.TXProtocol.bufDHDecodeKey;
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", Name, wSubVer));
            }

            fill_head(cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }
    }
}