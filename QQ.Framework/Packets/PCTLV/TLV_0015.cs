using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.ComputerGuid)]
    internal class TLV0015 : BaseTLV
    {
        public TLV0015()
        {
            Command = 0x0015;
            Name = "SSO2::TLV_ComputerGuid_0x15";
            WSubVer = 0x0001;
        }

        public byte[] Get_Tlv(QQUser user)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (WSubVer == 0x0001)
            {
                data.BeWrite(WSubVer); //wSubVer

                data.Write((byte) 0x01);
                var thisKey = user.TXProtocol.BufComputerId;
                data.BeWrite(CRC32.CRC32Reverse(thisKey));
                data.WriteKey(thisKey);

                data.Write((byte) 0x02);
                thisKey = user.TXProtocol.BufComputerIdEx;
                data.BeWrite(CRC32.CRC32Reverse(thisKey));
                data.WriteKey(thisKey);
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
    }
}