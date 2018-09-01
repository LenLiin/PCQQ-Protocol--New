using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0107 : BaseTLV
    {
        public TLV_0107()
        {
            this.cmd = 0x0107;
            this.Name = "SSO2::TLV_TicketInfo_0x107";
        }

        public void parser_tlv_0006(QQClient m_PCClient, BinaryReader buf)
        {
            byte[] buffer;
            ushort len;
            this.wSubVer = buf.BEReadUInt16(); //wSubVer
            if (this.wSubVer == 0x0001)
            {
                len = buf.BEReadUInt16();
                buffer = buf.ReadBytes(len);
                var bufTickStatus = new BinaryReader(new MemoryStream(buffer));
                var dwTGTServiceID = bufTickStatus.BEReadInt32();
                var dwTGTPriority = bufTickStatus.BEReadInt32();
                var dwTGTRefreshInterval = bufTickStatus.BEReadInt32();
                var dwTGTValidInterval = bufTickStatus.BEReadInt32();
                var dwTGTTryInterval = bufTickStatus.BEReadInt32();
                var wTGTTryCount = bufTickStatus.BEReadUInt16();

                buffer = buf.ReadBytes(16);
                 m_PCClient.QQUser.TXProtocol.bufTGT_GTKey= buffer;

                len = buf.BEReadUInt16();
                buffer = buf.ReadBytes(len);
                 m_PCClient.QQUser.TXProtocol.bufTGT= buffer;

                buffer = buf.ReadBytes(16);
                 m_PCClient.QQUser.TXProtocol.buf16bytesGTKey_ST= buffer;

                len = buf.BEReadUInt16();
                buffer = buf.ReadBytes(len);
                 m_PCClient.QQUser.TXProtocol.bufServiceTicket= buffer;

                len = buf.BEReadUInt16();
                buffer = buf.ReadBytes(len);
                var bufSTHttp = new BinaryReader(new MemoryStream(buffer));
                var bAllowPtlogin = bufSTHttp.ReadByte();
                buffer = bufSTHttp.ReadBytes(16);
                 m_PCClient.QQUser.TXProtocol.buf16bytesGTKey_STHttp= buffer;

                len = bufSTHttp.BEReadUInt16();
                buffer = bufSTHttp.ReadBytes(len);
                 m_PCClient.QQUser.TXProtocol.bufServiceTicketHttp= buffer;

                buffer = buf.ReadBytes(16);
                 m_PCClient.QQUser.TXProtocol.bufGTKey_TGTPwd= buffer;
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
        }
    }
}
