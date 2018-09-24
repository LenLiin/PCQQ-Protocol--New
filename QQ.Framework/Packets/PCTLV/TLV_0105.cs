using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    internal class TLV0105 : BaseTLV
    {
        [TlvTag(TlvTags.m_vec0x12c)]
        public TLV0105()
        {
            Command = 0x0105;
            Name = "TLV_m_vec0x12c";
            WSubVer = 0x0001;
        }

        public byte[] Get_Tlv(QQUser user)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (WSubVer == 0x0001)
            {
                data.BeWrite(WSubVer); //wSubVer
                data.Write(user.TXProtocol.XxooB);
                data.Write((byte) 2);
                data.BeUshortWrite(0x0014);
                data.BeWrite(0x01010010);
                data.Write(Util.RandomKey());
                data.BeUshortWrite(0x0014);
                data.BeWrite(0x01020010);
                data.Write(Util.RandomKey());
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
            WSubVer = buf.BeReadUInt16(); //wSubVer
            if (WSubVer == 0x0001)
            {
                buf.ReadByte(); //UNKNOWs
                var count = buf.ReadByte();
                for (var i = 0; i < count; i++)
                {
                    buf.ReadBytes(0x38); //buf
                }
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }
        }
    }
}