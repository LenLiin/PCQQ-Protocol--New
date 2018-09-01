using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0004 : BaseTLV
    {
        public TLV_0004()
        {
            cmd = 0x0004;
            Name = "SSO2::TLV_NonUinAccount_0x4";
            wSubVer = 0x0000;
        }

        public byte[] get_tlv_0004(QQClient m_PCClient)
        {
            if (m_PCClient.QQUser.QQ != 0)
            {
                return null;
            }

            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0000)
            {
                data.BEWrite(wSubVer); //wSubVer 
                var bufAccount = Util.HexStringToByteArray(Util.QQToHexString(m_PCClient.QQUser.QQ));
                data.BEWrite((ushort) bufAccount.Length); //账号长度
                data.Write(bufAccount); //账号
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", Name, wSubVer));
            }

            fill_head(cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }
    }
}