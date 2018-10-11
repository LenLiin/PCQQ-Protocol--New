namespace QQ.Framework.Packets.Receive.Login
{
    public class Receive_0X0839 : ReceivePacket
    {
        public Receive_0X0839(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
        }
    }
}