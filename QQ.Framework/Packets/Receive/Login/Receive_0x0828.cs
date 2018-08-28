using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Receive.Login
{
    public class Receive_0x0828 : ReceivePacket
    {
        public byte DataHead { get; set; }
        public Receive_0x0828(ByteBuffer byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_0828_rec_decr_key)
        {
        }

        protected override void ParseBody(ByteBuffer byteBuffer)
        {
            //密文
            byte[] CipherText = byteBuffer.ToByteArray();
            //明文
            bodyDecrypted = QQTea.Decrypt(CipherText, byteBuffer.Position, CipherText.Length - byteBuffer.Position - 1, user.QQ_0828_rec_decr_key);

            //提取数据
            ByteBuffer buf = new ByteBuffer(bodyDecrypted);

            if (GetPacketLength() == 407)
            {
                buf.GetByteArray(15);
                user.QQ_SessionKey = buf.GetByteArray(0x10);
            }
            else if (GetPacketLength() == 439|| GetPacketLength() == 527)
            {
                buf.GetByteArray(63);
                user.QQ_SessionKey = buf.GetByteArray(0x10);
            }
            else
            {
                throw new Exception("登录失败");
            }
        }
    }
}
