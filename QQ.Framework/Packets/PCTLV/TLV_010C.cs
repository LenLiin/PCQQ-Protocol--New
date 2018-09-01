using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;
using System;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_010C : BaseTLV
    {
        public TLV_010C()
        {
            this.cmd = 0x010C;
            this.Name = "TLV_010C";
        }

        public void parser_tlv_010C(QQClient m_PCClient, BinaryReader buf)
        {
            int len;
            this.wSubVer = buf.BEReadUInt16(); //wSubVer
            if (this.wSubVer == 0x0001)
            {
                 m_PCClient.QQUser.TXProtocol.SessionKey= buf.ReadBytes(16);
                var dwUin = buf.BEReadInt32();
//#if SHOWLOG
//                QQLog.Log("SessionKey:" + Util.ToHex(client.TXProtocol.SessionKey));
//#endif
                var dwClientIP = Util.GetIpStringFromBytes(buf.ReadBytes(4)); //IP地址
                m_PCClient.wClientPort = buf.BEReadUInt16();
                var dwServerTime = Util.GetDateTimeFromMillis(buf.BEReadInt32());
                var UNKNOW = buf.BEReadInt32();
                var cPassSeqID = buf.BEReadChar();
                var dwConnIP = buf.ReadBytes(4);
                var dwReLoginConnIP = buf.ReadBytes(4);
                var dwReLoginCtrlFlag = buf.BEReadInt32();

                len = buf.BEReadUInt16();
                var bufComputerIDSig = buf.ReadBytes(len);

                len = buf.ReadByte();
                var UNKNOW2 = buf.ReadBytes(len);

                len = buf.BEReadUInt16();
                var UNKNOW3 = buf.ReadBytes(len); 
                var a = new BinaryReader(new MemoryStream(UNKNOW3));
                a.ReadByte();
                var dwConnIP2 = a.ReadBytes(4);
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
        }
    }
}
