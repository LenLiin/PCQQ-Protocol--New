using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Receive.Login
{
    public class Receive_0x0825 : ReceivePacket
    {
        public Receive_0x0825(ByteBuffer byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_PACKET_0825KEY)
        {
        }
        protected override void ParseHeader(ByteBuffer buf)
        {
            base.ParseHeader(buf);
        }
        public byte DataHead { get; set; }
        protected override void ParseBody(ByteBuffer byteBuffer)
        {
            //密文
            byte[] CipherText = byteBuffer.ToByteArray();
            //明文
            if (!user.IsLoginRedirect)
            {
                bodyDecrypted = QQTea.Decrypt(CipherText, byteBuffer.Position, CipherText.Length - byteBuffer.Position - 1, user.QQ_PACKET_0825KEY);
            }
            else
            {
                bodyDecrypted = QQTea.Decrypt(CipherText, byteBuffer.Position, CipherText.Length - byteBuffer.Position - 1, user.QQ_PACKET_REDIRECTIONKEY);
            }
            if (bodyDecrypted == null)
                throw new Exception($"包内容解析出错，抛弃该包: {ToString()}");
            ByteBuffer buf = new ByteBuffer(bodyDecrypted);

            DataHead = buf.Get();
            buf.GetChar();//0112
            buf.GetChar();//0038
            user.QQ_0825Token = buf.GetByteArray(0x38);
            if (DataHead == 0xFE)
            {
                buf.GetByteArray(6);
                user.LoginTime = buf.GetByteArray(4);
                buf.GetByteArray(2);
                buf.GetByteArray(4);
                buf.GetByteArray(18);
                user.ServerIp = buf.GetByteArray(4);
                buf.GetByteArray(6);
            }
            else
            {
                buf.GetByteArray(6);
                user.LoginTime = buf.GetByteArray(4);
                buf.GetByteArray(2);
                buf.GetByteArray(4);
                buf.GetByteArray(6);
                user.ServerIp = buf.GetByteArray(4);
            }
            //从原始数据包提取加密包
            byteBuffer.GetByteArray(CipherText.Length - 1);
        }
    }
}
