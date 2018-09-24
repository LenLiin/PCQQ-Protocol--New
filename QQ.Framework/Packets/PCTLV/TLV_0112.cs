using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.SigIP2)]
    internal class TLV_0112 : BaseTLV
    {
        public TLV_0112()
        {
            cmd = 0x0112;
            Name = "SSO2::TLV_SigIP2_0x112";
        }

        public byte[] Get_Tlv(QQUser User)
        {
            var data = new BinaryWriter(new MemoryStream());
            data.Write(User.TXProtocol.bufSigClientAddr);
            fill_head(cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }

        public void Parser_Tlv(QQUser User, BinaryReader buf)
        {
            var _type = buf.BEReadUInt16(); //type
            var _length = buf.BEReadUInt16(); //length
            User.TXProtocol.bufSigClientAddr =
                buf.ReadBytes(_length); //bufSigClientAddr
        }
    }
}