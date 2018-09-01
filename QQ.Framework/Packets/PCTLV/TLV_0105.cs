using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0105 : BaseTLV
    {
        public TLV_0105()
        {
            this.cmd = 0x0105;
            this.Name = "TLV_m_vec0x12c";
            this.wSubVer = 0x0001;
        }

        public byte[] get_tlv_0105(QQClient m_PCClient)
        {
              var data = new BinaryWriter(new MemoryStream());
            if (this.wSubVer == 0x0001)
            {
                data.BEWrite(this.wSubVer); //wSubVer
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
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
            fill_head(this.cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }

        public void parser_tlv_0105(QQClient m_PCClient, BinaryReader buf)
        {
            this.wSubVer = buf.BEReadUInt16(); //wSubVer
            if (this.wSubVer == 0x0001)
            {
                buf.ReadByte();//UNKNOWs
                var Count = buf.ReadByte();
                for (int i = 0; i < Count; i++)
                {
                    buf.ReadBytes(0x38);//buf
                }
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
        }
    }
}
