using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.DeviceID)]
    internal class TLV_001F : BaseTLV
    {
        public TLV_001F()
        {
            cmd = 0x001F;
            Name = "TLV_DeviceID";
            wSubVer = 0x0001;
        }

        public byte[] Get_Tlv(QQUser User)
        {
            if (User.TXProtocol.bufDeviceID == null)
            {
                return new byte[] { };
            }

            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0001)
            {
                data.BEWrite(wSubVer); //wSubVer
                data.Write(User.TXProtocol.bufDeviceID);
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

        public void Parser_Tlv(QQUser User, BinaryReader buf)
        {
            var _type = buf.BEReadUInt16();//type
            var _length = buf.BEReadUInt16();//length
            if (wSubVer == 0x0001)
            {
                wSubVer = buf.BEReadUInt16(); //wSubVer
                User.TXProtocol.bufDeviceID =
                    buf.ReadBytes(_length - 2);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}