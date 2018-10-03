namespace QQ.Framework.Packets.Receive.Login
{
    public class Receive_0X0839 : ReceivePacket
    {
        public Receive_0X0839(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
        }
    }
}