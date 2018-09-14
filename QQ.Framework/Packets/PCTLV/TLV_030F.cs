using System.IO;
using System.Text;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    internal class TLV_030F : BaseTLV
    {
        public TLV_030F()
        {
            cmd = 0x030F;
            Name = "SSO2::TLV_SigIP2_0x112";
        }

        public byte[] get_tlv_030f(QQClient m_PCClient)
        {
            var data = new BinaryWriter(new MemoryStream());
            data.Write(Encoding.UTF8.GetBytes(m_PCClient.QQUser.bufComputerName));
            fill_head(cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }
    }
}