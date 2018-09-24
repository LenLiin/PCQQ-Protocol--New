namespace QQ.Framework.Packets.Receive.Message
{
    public class Receive_0X0319 : ReceivePacket
    {
        public Receive_0X0319(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
        }
    }
}