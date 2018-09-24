namespace QQ.Framework.Packets.Receive.Message
{
    /// <summary>
    ///     通用响应
    /// </summary>
    public class ReceiveCurrency : ReceivePacket
    {
        /// <summary>
        ///     通用响应
        /// </summary>
        public ReceiveCurrency(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
        }
    }
}