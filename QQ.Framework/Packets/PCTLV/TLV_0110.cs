using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.SigPic)]
    internal class TLV_0110 : BaseTLV
    {
        public TLV_0110()
        {
            cmd = 0x0110;
            Name = "SSO2::TLV_SigPic_0x110";
            wSubVer = 0x0001;
        }

        public byte[] Get_Tlv(QQUser User)
        {
            if (User.TXProtocol.bufSigPic == null)
            {
                return new byte[] { };
            }

            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0001)
            {
                data.BEWrite(wSubVer); //wSubVer
                data.WriteKey(User.TXProtocol.bufSigPic);
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

        public void Parser_Tlv(QQUser User, BinaryReader buf)
        {
            var _type = buf.BEReadUInt16(); //type
            var _length = buf.BEReadUInt16(); //length
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                User.TXProtocol.bufSigPic = buf.ReadBytes(buf.BEReadUInt16());
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}