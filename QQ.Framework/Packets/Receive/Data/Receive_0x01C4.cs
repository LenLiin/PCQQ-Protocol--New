namespace QQ.Framework.Packets.Receive.Data
{
    public class Receive_0X01C4 : ReceivePacket
    {
        public Receive_0X01C4(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
        }
    }
}