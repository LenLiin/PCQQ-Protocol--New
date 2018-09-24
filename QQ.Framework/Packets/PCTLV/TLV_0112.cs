using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.SigIP2)]
    internal class TLV0112 : BaseTLV
    {
        public TLV0112()
        {
            Command = 0x0112;
            Name = "SSO2::TLV_SigIP2_0x112";
        }

        public byte[] Get_Tlv(QQUser user)
        {
            var data = new BinaryWriter(new MemoryStream());
            data.Write(user.TXProtocol.BufSigClientAddr);
            FillHead(Command);
            FillBody(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            SetLength();
            return GetBuffer();
        }

        public void Parser_Tlv(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            user.TXProtocol.BufSigClientAddr =
                buf.ReadBytes(length); //bufSigClientAddr
        }
    }
}