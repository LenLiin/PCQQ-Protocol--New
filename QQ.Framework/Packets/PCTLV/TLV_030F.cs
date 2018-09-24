using System.IO;
using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.ComputerName)]
    internal class TLV_030F : BaseTLV
    {
        public TLV_030F()
        {
            cmd = 0x030F;
            Name = "SSO2::TLV_ComputerName";
        }

        public byte[] Get_Tlv(QQUser User)
        {
            var data = new BinaryWriter(new MemoryStream());
            data.BEWrite((ushort) Encoding.UTF8.GetBytes(User.TXProtocol.bufComputerName).Length);
            data.Write(Encoding.UTF8.GetBytes(User.TXProtocol.bufComputerName));
            fill_head(cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }
    }
}