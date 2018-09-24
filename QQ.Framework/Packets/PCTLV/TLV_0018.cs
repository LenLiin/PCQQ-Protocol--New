using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.Ping)]
    internal class TLV0018 : BaseTLV
    {
        public TLV0018()
        {
            Command = 0x0018;
            Name = "SSO2::TLV_Ping_0x18";
            WSubVer = 0x0001;
        }

        /// <summary>
        ///     Ping
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public byte[] Get_Tlv(QQUser user)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (WSubVer == 0x0001)
            {
                data.BeWrite(WSubVer); //wSubVer 
                data.BeWrite(user.TXProtocol.DwSsoVersion); //dwSSOVersion
                data.BeWrite(user.TXProtocol.DwServiceId); //dwServiceId
                data.BeWrite(user.TXProtocol.DwClientVer); //dwClientVer
                data.BeWrite((uint) user.QQ); //dwUin
                data.BeWrite(user.TXProtocol.WRedirectCount); //wRedirectCount 
                data.BeWrite((ushort) 0); //NullBuf
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
    }
}