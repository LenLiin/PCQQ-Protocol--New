namespace QQ.Framework.Packets.Receive.Message
{
    public class Receive_0x0360 : ReceivePacket
    {
        public Receive_0x0360(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(user.TXProtocol.SessionKey);
        }
    }
}