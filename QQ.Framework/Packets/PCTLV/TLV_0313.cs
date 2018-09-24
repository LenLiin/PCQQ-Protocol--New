using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.GUID_Ex)]
    internal class TLV_0313 : BaseTLV
    {
        public TLV_0313()
        {
            cmd = 0x0313;
            Name = "SSO2::TLV_GUID_Ex_0x313";
            wSubVer = 0x01;
        }

        public byte[] Get_Tlv(QQUser User)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x01)
            {
                data.Write((byte)1); 
                data.Write((byte)1); 
                data.Write((byte)2); 
                data.WriteKey(User.TXProtocol.bufMacGuid);
                data.BEWrite(2);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }

            fill_head(cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }
    }
}