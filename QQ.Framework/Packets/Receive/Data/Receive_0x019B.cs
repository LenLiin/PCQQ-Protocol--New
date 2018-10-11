namespace QQ.Framework.Packets.Receive.Data
{
    public class Receive_0X019B : ReceivePacket
    {
        public Receive_0X019B(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
        }
    }
}