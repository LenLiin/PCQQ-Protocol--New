namespace QQ.Framework.Packets.Receive.Message
{
    public class Receive_0X0058 : ReceivePacket
    {
        /// <summary>
        ///     心跳
        /// </summary>
        public Receive_0X0058(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
        }
    }
}