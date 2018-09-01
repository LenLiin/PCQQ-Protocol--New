using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0109 : BaseTLV
    {
        public TLV_0109()
        {
            this.cmd = 0x0109;
            this.Name = "SSO2::TLV_0xddReply_0x109";
        }

        public void parser_tlv_0006(QQClient m_PCClient, BinaryReader buf)
        {
            this.wSubVer = buf.BEReadUInt16(); //wSubVer
            if (this.wSubVer == 0x0001)
            {
                var buffer = buf.ReadBytes(16);
                 m_PCClient.QQUser.TXProtocol.bufSessionKey = buffer;

                var len = buf.BEReadUInt16();
                buffer = buf.ReadBytes(len);
                 m_PCClient.QQUser.TXProtocol.bufSigSession = buffer;

                len = buf.BEReadUInt16();
                buffer = buf.ReadBytes(len);
                 m_PCClient.QQUser.TXProtocol.bufPwdForConn = buffer;


                buf.BEReadUInt16();//bufBill
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
        }
    }
}
