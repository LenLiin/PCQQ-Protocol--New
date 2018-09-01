using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.Text;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0108 : BaseTLV
    {
        public TLV_0108()
        {
            this.cmd = 0x0108;
            this.Name = "SSO2::TLV_AccountBasicInfo_0x108";
        }

        public void parser_tlv_0108(QQClient m_PCClient, BinaryReader buf)
        {
            byte[] buffer;
            ushort len;
            this.wSubVer = buf.BEReadUInt16(); //wSubVer
            if (this.wSubVer == 0x0001)
            {
                len = buf.BEReadUInt16();
                buffer = buf.ReadBytes(len);
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

                var bufSTOther = bufAccountBasicInfo.ReadBytes((int)(bufAccountBasicInfo.BaseStream.Length- bufAccountBasicInfo.BaseStream.Position));
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
        }
    }
}
