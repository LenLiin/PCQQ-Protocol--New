namespace QQ.Framework.Packets.Receive.Message
{
    public class Receive_0x01C0 : ReceivePacket
    {
        public Receive_0x01C0(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(user.QQ_SessionKey);
        }
    }
}