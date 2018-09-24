using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.Misc_Flag)]
    internal class TLV0312 : BaseTLV
    {
        public TLV0312()
        {
            Command = 0x0312;
            Name = "SSO2::TLV_Misc_Flag_0x312";
        }

        public byte[] Get_Tlv(QQUser user)
        {
            var data = new BinaryWriter(new MemoryStream());
            data.Write((byte) 1);
            data.BeWrite(1);
            FillHead(Command);
            FillBody(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            SetLength();
            return GetBuffer();
        }
    }
}