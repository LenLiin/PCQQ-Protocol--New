using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    internal class TLV_0036 : BaseTLV
    {
        public TLV_0036()
        {
            cmd = 0x0036;
            Name = "SSO2::TLV_LoginReason_0x36";
            wSubVer = 0x0002;
        }

        public byte[] get_tlv_0036(QQClient m_PCClient)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0002)
            {
                data.BEWrite(wSubVer); //wSubVer
                data.BEWrite(1);
                data.BEWrite(0);
                data.BEWrite(0);
                data.BEWrite(0);
                data.BEWrite(0);
                data.Write(0);
                data.Write(0);
            }
            else if (wSubVer == 0x0001)
            {
                data.BEWrite(wSubVer); //wSubVer
                data.BEWrite(1);
                data.BEWrite(0);
                data.BEWrite(0);
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