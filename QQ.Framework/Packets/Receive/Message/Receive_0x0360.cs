namespace QQ.Framework.Packets.Receive.Message
{
    public class Receive_0X0360 : ReceivePacket
    {
        public Receive_0X0360(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
        }
    }
}