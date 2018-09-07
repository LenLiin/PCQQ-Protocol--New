using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0008 : BaseTLV
    {
        public TLV_0008()
        {
            cmd = 0x0008;
            Name = "SSO2::TLV_TimeZone_0x8";
            wSubVer = 0x0001;
        }

        public byte[] get_tlv_8(QQClient m_PCClient)
        {
            //if (PCQQGlobal.dwLocaleID == 0x00000804 && PCQQGlobal.wTimeZoneoffsetMin == 0x01E0)
            //{
            //    return null;
            //}
            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0001)
            {
                data.BEWrite(wSubVer); //wSubVer 
                data.BEWrite(0x00000804); //此乃LCID
                data.BEWrite(0x01E0); //此乃时区信息
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