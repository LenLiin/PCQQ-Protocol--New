using System.IO;
using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.ComputerName)]
    internal class TLV030F : BaseTLV
    {
        public TLV030F()
        {
            Command = 0x030F;
            Name = "SSO2::TLV_ComputerName";
        }

        public byte[] Get_Tlv(QQUser user)
        {
            var data = new BinaryWriter(new MemoryStream());
            data.BeWrite((ushort) Encoding.UTF8.GetBytes(user.TXProtocol.BufComputerName).Length);
            data.Write(Encoding.UTF8.GetBytes(user.TXProtocol.BufComputerName));
            FillHead(Command);
            FillBody(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            SetLength();
            return GetBuffer();
        }
    }
}