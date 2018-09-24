using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.PingRedirect)]
    internal class TLV_000C : BaseTLV
    {
        public TLV_000C()
        {
            cmd = 0x000C;
            Name = "SSO2::TLV_PingRedirect_0xC";
            wSubVer = 0x0002;
        }

        public byte[] Get_Tlv(QQUser User)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0002)
            {
                data.BEWrite(wSubVer); //wSubVer
                data.BEWrite((ushort) 0);
                data.BEWrite(User.TXProtocol.dwIDC);
                data.BEWrite(User.TXProtocol.dwISP);
                data.Write(Util.IPStringToByteArray(User.TXProtocol.dwServerIP));
                data.BEWrite(User.TXProtocol.wServerPort);
                data.BEWrite(0);
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
            if (wSubVer == 0x0002)
            {
                buf.BEReadUInt16();
                User.TXProtocol.dwIDC = buf.BEReadUInt32(); /*dwIDC =*/
                User.TXProtocol.dwISP = buf.BEReadUInt32(); /*dwISP =*/
                User.TXProtocol.dwRedirectIP = Util.GetIpStringFromBytes(buf.ReadBytes(4)); /*dwRedirectIP =*/
                User.TXProtocol.wRedirectPort = buf.BEReadUInt16(); /*wRedirectPort =*/
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}