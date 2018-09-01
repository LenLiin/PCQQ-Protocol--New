using QQ.Framework;
using QQ.Framework.Utils;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_001A : BaseTLV
    {
        public TLV_001A()
        {
            cmd = 0x001A;
            Name = "SSO2::TLV_GTKeyTGTGTCryptedData_0x1a";
        }

        public byte[] get_tlv_001A(QQClient m_PCClient)
        {
            var data = new TLV_0015().get_tlv_0015(m_PCClient);

            var encode = QQTea.Encrypt(data, m_PCClient.QQUser.TXProtocol.bufTGTGTKey);

            fill_head(cmd);
            fill_body(encode, encode.Length);
            set_length();
            return get_buf();
        }
    }
}