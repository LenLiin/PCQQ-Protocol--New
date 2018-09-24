using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.TicketInfo)]
    internal class TLV_0107 : BaseTLV
    {
        public TLV_0107()
        {
            cmd = 0x0107;
            Name = "SSO2::TLV_TicketInfo_0x107";
        }

        public void Parser_Tlv(QQUser User, BinaryReader buf)
        {
            var _type = buf.BEReadUInt16(); //type
            var _length = buf.BEReadUInt16(); //length
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                var len = buf.BEReadUInt16();
                var buffer = buf.ReadBytes(len);
                var bufTickStatus = new BinaryReader(new MemoryStream(buffer));
                var dwTGTServiceID = bufTickStatus.BEReadInt32();
                var dwTGTPriority = bufTickStatus.BEReadInt32();
                var dwTGTRefreshInterval = bufTickStatus.BEReadInt32();
                var dwTGTValidInterval = bufTickStatus.BEReadInt32();
                var dwTGTTryInterval = bufTickStatus.BEReadInt32();
                var wTGTTryCount = bufTickStatus.BEReadUInt16();

                buffer = buf.ReadBytes(16);
                User.TXProtocol.bufTGT_GTKey = buffer;

                len = buf.BEReadUInt16();
                buffer = buf.ReadBytes(len);
                User.TXProtocol.bufTGT = buffer;

                buffer = buf.ReadBytes(16);
                User.TXProtocol.buf16bytesGTKey_ST = buffer;

                len = buf.BEReadUInt16();
                buffer = buf.ReadBytes(len);
                User.TXProtocol.bufServiceTicket = buffer;

                len = buf.BEReadUInt16();
                buffer = buf.ReadBytes(len);
                var bufSTHttp = new BinaryReader(new MemoryStream(buffer));
                var bAllowPtlogin = bufSTHttp.ReadByte();
                buffer = bufSTHttp.ReadBytes(16);
                User.TXProtocol.buf16bytesGTKey_STHttp = buffer;

                len = bufSTHttp.BEReadUInt16();
                buffer = bufSTHttp.ReadBytes(len);
                User.TXProtocol.bufServiceTicketHttp = buffer;

                buffer = buf.ReadBytes(16);
                User.TXProtocol.bufGTKey_TGTPwd = buffer;
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}