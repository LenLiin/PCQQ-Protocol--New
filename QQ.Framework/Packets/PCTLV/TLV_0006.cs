using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0006 : BaseTLV
    {
        public TLV_0006()
        {
            this.cmd = 0x0006;
            this.Name = "SSO2::TLV_TGTGT_0x6";
            this.wSubVer = 0x0002;
        }

        public byte[] get_tlv_0006(QQClient m_PCClient)
        {
            if (this.wSubVer == 0x0002)
            {
                if (m_PCClient.QQUser.TXProtocol.bufTGTGT == null)
                {
                    var data = new BinaryWriter(new MemoryStream());
                    data.BEWrite(new Random(Guid.NewGuid().GetHashCode()).Next());//随机4字节??
                    data.BEWrite(this.wSubVer); //wSubVer
                    data.BEWrite((uint)m_PCClient.QQUser.QQ); //QQ号码
                    data.BEWrite(QQGlobal.dwSSOVersion);
                    data.BEWrite(QQGlobal.dwServiceId);
                    data.BEWrite(QQGlobal.dwClientVer);
                    data.BEWrite(0);
                    data.Write(m_PCClient.QQUser.bRememberPwdLogin);
                    data.Write(m_PCClient.QQUser.MD51); //密码的一次MD5值，服务器用该MD5值验证用户密码是否正确
                    data.Write(m_PCClient.QQUser.LoginTime); //登录时间
                    data.Write(new byte[13]); //固定13字节
                    data.Write(m_PCClient.QQUser.IP);//IP地址
                    data.BEWrite(QQGlobal.dwISP); //dwISP
                    data.BEWrite(QQGlobal.dwIDC); //dwIDC
                    data.Write(m_PCClient.QQUser.TXProtocol.bufComputerIDEx); //机器码
                    data.Write(m_PCClient.QQUser.QQ_PACKET_TgtgtKey); //00DD临时密钥(通过验证时客户端用该密钥解密服务端发送回来的数据)

                    m_PCClient.QQUser.TXProtocol.bufTGTGT = QQTea.Encrypt(data.BaseStream.ToBytesArray(), m_PCClient.QQUser.Md52());
                }
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
            var tlv = new BinaryWriter(new MemoryStream());
            tlv.Write(m_PCClient.QQUser.TXProtocol.bufTGTGT);
            fill_head(this.cmd);
            fill_body(tlv.BaseStream.ToBytesArray(), tlv.BaseStream.Length);
            set_length();
            return get_buf();
        }

        public void parser_tlv_0006(QQClient m_PCClient, BinaryReader buf)
        {
            m_PCClient.QQUser.TXProtocol.bufTGTGT = buf.ReadBytes((int)(buf.BaseStream.Length- buf.BaseStream.Position));
        }
    }
}
