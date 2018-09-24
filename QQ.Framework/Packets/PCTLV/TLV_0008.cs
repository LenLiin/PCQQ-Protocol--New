using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.TimeZone)]
    internal class TLV0008 : BaseTLV
    {
        public TLV0008()
        {
            Command = 0x0008;
            Name = "SSO2::TLV_TimeZone_0x8";
            WSubVer = 0x0001;
        }

        public byte[] Get_Tlv(QQUser user)
        {
            //if (PCQQGlobal.dwLocaleID == 0x00000804 && PCQQGlobal.wTimeZoneoffsetMin == 0x01E0)
            //{
            //    return null;
            //}
            var data = new BinaryWriter(new MemoryStream());
            if (WSubVer == 0x0001)
            {
                data.BeWrite(WSubVer); //wSubVer 
                data.BeWrite(0x00000804); //此乃LCID
                data.BeWrite(0x01E0); //此乃时区信息
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