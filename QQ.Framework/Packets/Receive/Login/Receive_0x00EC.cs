namespace QQ.Framework.Packets.Receive.Login
{
    /// <summary>
    ///     改变在线状态
    /// </summary>
    public class Receive_0X00Ec : ReceivePacket
    {
        /// <summary>
        ///     改变在线状态
        /// </summary>
        public Receive_0X00Ec(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
        }
    }
}