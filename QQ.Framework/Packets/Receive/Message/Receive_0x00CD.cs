namespace QQ.Framework.Packets.Receive.Message
{
    public class Receive_0X00Cd : ReceivePacket
    {
        /// <summary>
        ///     发好友消息回复包
        /// </summary>
        public Receive_0X00Cd(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
        }
    }
}