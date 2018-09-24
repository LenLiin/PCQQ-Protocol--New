using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags._0x0508)]
    internal class TLV0508 : BaseTLV
    {
        public TLV0508()
        {
            Command = 0x0508;
            Name = "SSO2::TLV_0508";
        }

        public byte[] Get_Tlv(QQUser user)
        {
            var data = new BinaryWriter(new MemoryStream());
            data.Write((byte) 1);
            data.BeWrite(0);


            FillHead(Command);
            FillBody(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            SetLength();
            return GetBuffer();
        }

        public void Parser_Tlv(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            buf.ReadBytes(length);
        }
    }
}