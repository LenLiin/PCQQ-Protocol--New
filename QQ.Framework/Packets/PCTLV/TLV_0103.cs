using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.SID)]
    internal class TLV0103 : BaseTLV
    {
        public TLV0103()
        {
            Command = 0x0103;
            Name = "SSO2::TLV_SID_0x103";
            WSubVer = 0x0001;
        }

        public byte[] Get_Tlv(QQUser user)
        {
            if (user.TXProtocol.BufSid == null || user.TXProtocol.BufSid.Length == 0)
            {
                return null;
            }

            var data = new BinaryWriter(new MemoryStream());
            data.BeWrite(WSubVer);
            data.WriteKey(user.TXProtocol.BufSid);
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
                var len = buf.BeReadUInt16();
                user.TXProtocol.BufSid = buf.ReadBytes(len);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }
        }
    }
}