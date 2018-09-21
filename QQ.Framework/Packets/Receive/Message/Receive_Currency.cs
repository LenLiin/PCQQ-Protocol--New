namespace QQ.Framework.Packets.Receive.Message
{
    /// <summary>
    ///     通用响应
    /// </summary>
    public class Receive_Currency : ReceivePacket
    {
        /// <summary>
        ///     通用响应
        /// </summary>
        public Receive_Currency(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(user.TXProtocol.SessionKey);
        }
    }
}