using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.LoginReason)]
    internal class TLV_0036 : BaseTLV
    {
        public TLV_0036()
        {
            cmd = 0x0036;
            Name = "SSO2::TLV_LoginReason_0x36";
            wSubVer = 0x0002;
        }
        /// <summary>
        /// LoginReason
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public byte[] Get_Tlv(QQUser User)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0002)
            {
                data.BEWrite(wSubVer); //wSubVer
                data.BEUshortWrite(1);
                data.BEUshortWrite(0);
                data.BEUshortWrite(1);
                data.BEUshortWrite(0);
                data.BEUshortWrite(0);
                data.BEUshortWrite(0);
                data.BEUshortWrite(0);
                data.BEUshortWrite(0);
            }
            else if (wSubVer == 0x0001)
            {
                data.BEWrite(wSubVer); //wSubVer
                data.BEUshortWrite(1);
                data.BEUshortWrite(0);
                data.BEUshortWrite(0);
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