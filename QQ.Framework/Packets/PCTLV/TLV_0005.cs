using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.Uin)]
    internal class TLV0005 : BaseTLV
    {
        public TLV0005()
        {
            Command = 0x0005;
            Name = "SSO2::TLV_Uin_0x5";
            WSubVer = 0x0002;
        }

        public byte[] Get_Tlv(QQUser user)
        {
            if (user.QQ == 0)
            {
                return null;
            }

            var buf = new BinaryWriter(new MemoryStream());
            if (WSubVer == 0x0002)
            {
                buf.BeWrite(WSubVer);
                buf.BeWrite((uint) user.QQ);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }

            FillHead(Command);
            FillBody(buf.BaseStream.ToBytesArray(), buf.BaseStream.Length);
            SetLength();
            return GetBuffer();
        }
    }
}