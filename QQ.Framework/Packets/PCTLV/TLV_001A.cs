using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.GTKeyTGTGTCryptedData)]
    internal class TLV_001A : BaseTLV
    {
        public TLV_001A()
        {
            cmd = 0x001A;
            Name = "SSO2::TLV_GTKeyTGTGTCryptedData_0x1a";
        }

        public byte[] Get_Tlv(QQUser User)
        {
            var data = new TLV_0015().Get_Tlv(User);

            var encode = QQTea.Encrypt(data, User.TXProtocol.bufTGTGTKey);

            fill_head(cmd);
            fill_body(encode, encode.Length);
            set_length();
            return get_buf();
        }
    }
}