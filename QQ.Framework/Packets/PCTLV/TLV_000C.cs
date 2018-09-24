using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.PingRedirect)]
    internal class TLV000C : BaseTLV
    {
        public TLV000C()
        {
            Command = 0x000C;
            Name = "SSO2::TLV_PingRedirect_0xC";
            WSubVer = 0x0002;
        }

        public byte[] Get_Tlv(QQUser user)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (WSubVer == 0x0002)
            {
                data.BeWrite(WSubVer); //wSubVer
                data.BeWrite((ushort) 0);
                data.BeWrite(user.TXProtocol.DwIdc);
                data.BeWrite(user.TXProtocol.DwIsp);
                data.Write(Util.IPStringToByteArray(user.TXProtocol.DwServerIP));
                data.BeWrite(user.TXProtocol.WServerPort);
                data.BeWrite(0);
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
            if (WSubVer == 0x0002)
            {
                buf.BeReadUInt16();
                user.TXProtocol.DwIdc = buf.BeReadUInt32(); /*dwIDC =*/
                user.TXProtocol.DwIsp = buf.BeReadUInt32(); /*dwISP =*/
                user.TXProtocol.DwRedirectIP = Util.GetIpStringFromBytes(buf.ReadBytes(4)); /*dwRedirectIP =*/
                user.TXProtocol.WRedirectPort = buf.BeReadUInt16(); /*wRedirectPort =*/
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }
        }
    }
}