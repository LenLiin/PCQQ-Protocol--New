namespace QQ.Framework.Packets.Receive.Message
{
    /// <summary>
    ///     发送消息应答
    /// </summary>
    public class Receive_0X0002 : ReceivePacket
    {
        /// <summary>
        /// </summary>
        public Receive_0X0002(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
            Reader.ReadBytes(4);
        }
    }
}