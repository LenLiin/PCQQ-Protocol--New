using System;
using System.IO;
using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.AccountBasicInfo)]
    internal class TLV0108 : BaseTLV
    {
        public TLV0108()
        {
            Command = 0x0108;
            Name = "SSO2::TLV_AccountBasicInfo_0x108";
        }

        public void Parser_Tlv(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            WSubVer = buf.BeReadUInt16(); //wSubVer
            if (WSubVer == 0x0001)
            {
                var len = buf.BeReadUInt16();
                var buffer = buf.ReadBytes(len);
                var bufAccountBasicInfo = new BinaryReader(new MemoryStream(buffer));

                len = bufAccountBasicInfo.BeReadUInt16();
                buffer = bufAccountBasicInfo.ReadBytes(len);
                var info = new BinaryReader(new MemoryStream(buffer));
                var wSsoAccountWFaceIndex = info.BeReadUInt16();
                len = info.ReadByte();
                if (len > 0)
                {
                    user.NickName = Encoding.UTF8.GetString(info.ReadBytes(len));
                }

                user.Gender = info.ReadByte();
                var dwSsoAccountDwUinFlag = info.BeReadUInt32();
                user.Age = info.ReadByte();

                var bufStOther =
                    bufAccountBasicInfo.ReadBytes(
                        (int) (bufAccountBasicInfo.BaseStream.Length - bufAccountBasicInfo.BaseStream.Position));
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }
        }
    }
}