using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Receive.Data
{
    public class Receive_0x005C : ReceivePacket
    {
        /// <summary>
        /// 改变在线状态
        /// </summary>
        public Receive_0x005C(ByteBuffer byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_SessionKey)
        {
        }
        protected override void ParseBody(ByteBuffer byteBuffer)
        {
            //密文
            byte[] CipherText = byteBuffer.ToByteArray();
            //明文
            bodyDecrypted = QQTea.Decrypt(CipherText, byteBuffer.Position, CipherText.Length - byteBuffer.Position - 1, user.QQ_SessionKey);
            //提取数据
            ByteBuffer buf = new ByteBuffer(bodyDecrypted);

        }
    }
}
