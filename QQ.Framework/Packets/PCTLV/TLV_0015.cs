using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.ComputerGuid)]
    internal class TLV_0015 : BaseTLV
    {
        public TLV_0015()
        {
            cmd = 0x0015;
            Name = "SSO2::TLV_ComputerGuid_0x15";
            wSubVer = 0x0001;
        }

        public byte[] Get_Tlv(QQUser User)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0001)
            {
                data.BEWrite(wSubVer); //wSubVer

                data.Write((byte)0x01);
                var thisKey = User.TXProtocol.bufComputerID;
                data.BEWrite(CRC32.CRC32Reverse(thisKey));
                data.WriteKey(thisKey);

                data.Write((byte)0x02);
                thisKey = User.TXProtocol.bufComputerIDEx;
                data.BEWrite(CRC32.CRC32Reverse(thisKey));
                data.WriteKey(thisKey);
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