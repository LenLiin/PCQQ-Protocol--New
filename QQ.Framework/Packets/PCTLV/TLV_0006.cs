using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.TGTGT)]
    internal class TLV_0006 : BaseTLV
    {
        public TLV_0006()
        {
            cmd = 0x0006;
            Name = "SSO2::TLV_TGTGT_0x6";
            wSubVer = 0x0002;
        }

        public byte[] Get_Tlv(QQUser User)
        {
            if (wSubVer == 0x0002)
            {
                if (User.TXProtocol.bufTGTGT == null)
                {
                    var data = new BinaryWriter(new MemoryStream());
                    data.BEWrite(new Random(Guid.NewGuid().GetHashCode()).Next()); //随机4字节??
                    data.BEWrite(wSubVer); //wSubVer
                    data.BEWrite(User.QQ); //QQ号码
                    data.BEWrite(User.TXProtocol.dwSSOVersion);
                    data.BEWrite(User.TXProtocol.dwServiceId);
                    data.BEWrite(User.TXProtocol.dwClientVer);
                    data.BEWrite((ushort)0);
                    data.Write(User.TXProtocol.bRememberPwdLogin);
                    data.Write(User.MD51); //密码的一次MD5值，服务器用该MD5值验证用户密码是否正确
                    data.BEWrite(User.TXProtocol.dwServerTime); //登录时间
                    data.Write(new byte[13]); //固定13字节
                    data.Write(Util.IPStringToByteArray(User.TXProtocol.dwClientIP)); //IP地址
                    data.BEWrite(User.TXProtocol.dwISP); //dwISP
                    data.BEWrite(User.TXProtocol.dwIDC); //dwIDC
                    data.WriteKey(User.TXProtocol.bufComputerIDEx); //机器码
                    data.Write(User.TXProtocol.bufTGTGTKey); //00DD临时密钥(通过验证时客户端用该密钥解密服务端发送回来的数据)

                    User.TXProtocol.bufTGTGT =
                        QQTea.Encrypt(data.BaseStream.ToBytesArray(), User.Md52());
                }
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }

            var tlv = new BinaryWriter(new MemoryStream());
            tlv.Write(User.TXProtocol.bufTGTGT);
            fill_head(cmd);
            fill_body(tlv.BaseStream.ToBytesArray(), tlv.BaseStream.Length);
            set_length();
            return get_buf();
        }

        public void Parser_Tlv(QQUser User, BinaryReader buf)
        {
            var _type = buf.BEReadUInt16();//type
            var _length = buf.BEReadUInt16();//length
            User.TXProtocol.bufTGTGT =
                buf.ReadBytes(_length);
        }
    }
}