using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.Ping_Strategy)]
    internal class TLV_0309 : BaseTLV
    {
        public TLV_0309()
        {
            cmd = 0x0309;
            Name = "SSO2::TLV_Ping_Strategy_0x309";
            wSubVer = 0x0001;
        }

        /// <summary>
        ///     Ping_Strategy
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public byte[] Get_Tlv(QQUser User)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0001)
            {
                data.BEWrite(wSubVer); //wSubVer
                data.Write(Util.IPStringToByteArray(User.TXProtocol.dwServerIP)); //LastServerIP - 服务器最后的登录IP，可以为0
                data.Write((byte) User.TXProtocol.RedirectIP.Count); //cRedirectCount - 重定向的次数（IP的数量）
                foreach (var ip in User.TXProtocol.RedirectIP)
                {
                    data.Write(ip);
                }

                data.Write(User.TXProtocol.cPingType); //cPingType 
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


        public byte GetPingType(int val)
        {
            switch (val)
            {
                case 10: //0xA
                case 20: //0x14
                    return 1;
                case 30: //0x1E
                    return 2;
                case 40: //0x28
                    return 3;
                case 50: //0x32
                case 60: //0x3C
                    return 4;
                case 70: //0x46
                    return 6;
                case 25: //0x19
                    return 7;
                default:
                    return 4;
            }
        }
    }
}