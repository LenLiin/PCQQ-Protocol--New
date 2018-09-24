using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.GTKeyTGTGTCryptedData)]
    internal class TLV001A : BaseTLV
    {
        public TLV001A()
        {
            Command = 0x001A;
            Name = "SSO2::TLV_GTKeyTGTGTCryptedData_0x1a";
        }

        public byte[] Get_Tlv(QQUser user)
        {
            var data = new TLV0015().Get_Tlv(user);

            var encode = QQTea.Encrypt(data, user.TXProtocol.BufTgtgtKey);

            FillHead(Command);
            FillBody(encode, encode.Length);
            SetLength();
            return GetBuffer();
        }
    }
}