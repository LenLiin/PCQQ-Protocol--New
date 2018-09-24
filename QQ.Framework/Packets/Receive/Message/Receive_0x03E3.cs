namespace QQ.Framework.Packets.Receive.Message
{
    /// <summary>
    ///     点赞回复包
    /// </summary>
    public class Receive_0X03E3 : ReceivePacket
    {
        /// <summary>
        ///     点赞回复包
        /// </summary>
        public Receive_0X03E3(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
        }
    }
}