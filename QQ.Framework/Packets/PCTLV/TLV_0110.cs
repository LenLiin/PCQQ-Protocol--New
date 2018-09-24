using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.SigPic)]
    internal class TLV0110 : BaseTLV
    {
        public TLV0110()
        {
            Command = 0x0110;
            Name = "SSO2::TLV_SigPic_0x110";
            WSubVer = 0x0001;
        }

        public byte[] Get_Tlv(QQUser user)
        {
            if (user.TXProtocol.BufSigPic == null)
            {
                return new byte[] { };
            }

            var data = new BinaryWriter(new MemoryStream());
            if (WSubVer == 0x0001)
            {
                data.BeWrite(WSubVer); //wSubVer
                data.WriteKey(user.TXProtocol.BufSigPic);
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

        public void Parser_Tlv(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            WSubVer = buf.BeReadUInt16(); //wSubVer
            if (WSubVer == 0x0001)
            {
                user.TXProtocol.BufSigPic = buf.ReadBytes(buf.BeReadUInt16());
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }
        }
    }
}