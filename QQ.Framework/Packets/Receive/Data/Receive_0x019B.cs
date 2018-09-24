using System;
using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Receive.Data
{
    public class Receive_0x019B : ReceivePacket
    {
        /// <summary>
        ///     改变在线状态
        /// </summary>
        public Receive_0x019B(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
        }
    }
}