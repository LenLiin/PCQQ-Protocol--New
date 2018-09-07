using System;
using System.IO;
using System.Text;
using QQ.Framework;
using QQ.Framework.Utils;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0108 : BaseTLV
    {
        public TLV_0108()
        {
            cmd = 0x0108;
            Name = "SSO2::TLV_AccountBasicInfo_0x108";
        }

        public void parser_tlv_0108(QQClient m_PCClient, BinaryReader buf)
        {
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                var len = buf.BEReadUInt16();
                var buffer = buf.ReadBytes(len);
                var bufAccountBasicInfo = new BinaryReader(new MemoryStream(buffer));

                len = bufAccountBasicInfo.BEReadUInt16();
                buffer = bufAccountBasicInfo.ReadBytes(len);
                var info = new BinaryReader(new MemoryStream(buffer));
                var wSSO_Account_wFaceIndex = info.BEReadUInt16();
                len = info.ReadByte();
                if (len > 0)
                {
                    m_PCClient.QQUser.NickName = Encoding.UTF8.GetString(info.ReadBytes(len));
                }

                var cSSO_Account_cGender = info.ReadByte();
                var dwSSO_Account_dwUinFlag = info.BEReadUInt32();
                m_PCClient.QQUser.Age = info.ReadByte();

                var bufSTOther =
                    bufAccountBasicInfo.ReadBytes(
                        (int) (bufAccountBasicInfo.BaseStream.Length - bufAccountBasicInfo.BaseStream.Position));
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}