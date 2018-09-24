using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.DeviceID)]
    internal class TLV001F : BaseTLV
    {
        public TLV001F()
        {
            Command = 0x001F;
            Name = "TLV_DeviceID";
            WSubVer = 0x0001;
        }

        public byte[] Get_Tlv(QQUser user)
        {
            if (user.TXProtocol.BufDeviceId == null)
            {
                return new byte[] { };
            }

            var data = new BinaryWriter(new MemoryStream());
            if (WSubVer == 0x0001)
            {
                data.BeWrite(WSubVer); //wSubVer
                data.Write(user.TXProtocol.BufDeviceId);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }

            FillHead(Command);
            FillBody(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            SetLength();
            return GetBuffer();
        }

        public void Parser_Tlv(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            if (WSubVer == 0x0001)
            {
                WSubVer = buf.BeReadUInt16(); //wSubVer
                user.TXProtocol.BufDeviceId =
                    buf.ReadBytes(length - 2);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }
        }
    }
}