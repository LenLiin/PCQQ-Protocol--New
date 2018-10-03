namespace QQ.Framework.Packets.Receive.Data
{
    public class Receive_0X00D8 : ReceivePacket
    {
        public Receive_0X00D8(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
        }
    }
}