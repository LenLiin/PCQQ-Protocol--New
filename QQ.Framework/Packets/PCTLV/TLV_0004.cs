using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.NonUinAccount)]
    internal class TLV_0004 : BaseTLV
    {
        public TLV_0004()
        {
            cmd = 0x0004;
            Name = "SSO2::TLV_NonUinAccount_0x4";
            wSubVer = 0x0000;
        }

        public byte[] Get_Tlv(QQUser User)
        {
            if (User.QQ != 0)
            {
                return null;
            }

            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0000)
            {
                data.BEWrite(wSubVer); //wSubVer 
                var bufAccount = Util.HexStringToByteArray(Util.NumToHexString(User.QQ));
                data.BEWrite((ushort) bufAccount.Length); //账号长度
                data.Write(bufAccount); //账号
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