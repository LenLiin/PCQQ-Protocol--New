using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    internal class TLV_0105 : BaseTLV
    {
        [TlvTag(TlvTags.m_vec0x12c)]
        public TLV_0105()
        {
            cmd = 0x0105;
            Name = "TLV_m_vec0x12c";
            wSubVer = 0x0001;
        }

        public byte[] Get_Tlv(QQUser User)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0001)
            {
                data.BEWrite(wSubVer); //wSubVer
                data.Write(User.TXProtocol.xxoo_b);
                data.Write((byte)2);
                data.BEUshortWrite(0x0014);
                data.BEWrite(0x01010010);
                data.Write(Util.RandomKey());
                data.BEUshortWrite(0x0014);
                data.BEWrite(0x01020010);
                data.Write(Util.RandomKey());
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
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                buf.ReadByte(); //UNKNOWs
                var Count = buf.ReadByte();
                for (var i = 0; i < Count; i++)
                {
                    buf.ReadBytes(0x38); //buf
                }
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}