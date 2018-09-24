using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.SID)]
    internal class TLV_0103 : BaseTLV
    {
        public TLV_0103()
        {
            cmd = 0x0103;
            Name = "SSO2::TLV_SID_0x103";
            wSubVer = 0x0001;
        }

        public byte[] Get_Tlv(QQUser User)
        {
            if (User.TXProtocol.bufSID == null || User.TXProtocol.bufSID.Length == 0)
            {
                return null;
            }

            var data = new BinaryWriter(new MemoryStream());
            data.BEWrite(wSubVer);
            data.WriteKey(User.TXProtocol.bufSID);
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
                var len = buf.BEReadUInt16();
                User.TXProtocol.bufSID = buf.ReadBytes(len);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}