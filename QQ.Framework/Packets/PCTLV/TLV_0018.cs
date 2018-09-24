using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.Ping)]
    internal class TLV_0018 : BaseTLV
    {
        public TLV_0018()
        {
            cmd = 0x0018;
            Name = "SSO2::TLV_Ping_0x18";
            wSubVer = 0x0001;
        }
        /// <summary>
        /// Ping
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public byte[] Get_Tlv(QQUser User)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0001)
            {
                data.BEWrite(wSubVer); //wSubVer 
                data.BEWrite(User.TXProtocol.dwSSOVersion); //dwSSOVersion
                data.BEWrite(User.TXProtocol.dwServiceId); //dwServiceId
                data.BEWrite(User.TXProtocol.dwClientVer); //dwClientVer
                data.BEWrite((uint)User.QQ); //dwUin
                data.BEWrite(User.TXProtocol.wRedirectCount); //wRedirectCount 
                data.BEWrite((ushort)0); //NullBuf
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
    }
}