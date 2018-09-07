using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0105 : BaseTLV
    {
        public TLV_0105()
        {
            cmd = 0x0105;
            Name = "TLV_m_vec0x12c";
            wSubVer = 0x0001;
        }

        public byte[] get_tlv_0105(QQClient m_PCClient)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0001)
            {
                data.BEWrite(wSubVer); //wSubVer
                data.Write(1);
                data.Write(2);
                data.BEWrite(0x0014);
                data.BEWrite(0x01010010);
                data.Write(Util.RandomKey());
                data.BEWrite(0x0014);
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

        public void parser_tlv_0105(QQClient m_PCClient, BinaryReader buf)
        {
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