using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.NonUinAccount)]
    internal class TLV0004 : BaseTLV
    {
        public TLV0004()
        {
            Command = 0x0004;
            Name = "SSO2::TLV_NonUinAccount_0x4";
            WSubVer = 0x0000;
        }

        public byte[] Get_Tlv(QQUser user)
        {
            if (user.QQ != 0)
            {
                return null;
            }

            var data = new BinaryWriter(new MemoryStream());
            if (WSubVer == 0x0000)
            {
                data.BeWrite(WSubVer); //wSubVer 
                var bufAccount = Util.HexStringToByteArray(Util.NumToHexString(user.QQ));
                data.BeWrite((ushort) bufAccount.Length); //账号长度
                data.Write(bufAccount); //账号
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