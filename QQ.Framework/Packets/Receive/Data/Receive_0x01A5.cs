namespace QQ.Framework.Packets.Receive.Data
{
    public class Receive_0X01A5 : ReceivePacket
    {
        public Receive_0X01A5(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
        }
    }
}