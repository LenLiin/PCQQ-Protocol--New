namespace QQ.Framework.Packets.Receive.Data
{
    public class Receive_0X0126 : ReceivePacket
    {
        public Receive_0X0126(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
        }
    }
}