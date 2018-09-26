using System;
using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Receive.Data
{
    public class Receive_0X01A5 : ReceivePacket
    {
        public Receive_0X01A5(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
        }
    }
}