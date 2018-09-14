using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    internal class TLV_0312 : BaseTLV
    {
        public TLV_0312()
        {
            cmd = 0x0312;
            Name = "SSO2::TLV_Misc_Flag_0x312";
        }

        public byte[] get_tlv_0312(QQClient m_PCClient)
        {
            var data = new BinaryWriter(new MemoryStream());
            data.Write(1);
            data.BEWrite(0);
            fill_head(cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }
    }
}