namespace QQ.Framework.Packets.Receive.Data
{
    public class Receive_0x005C : ReceivePacket
    {
        /// <summary>
        ///     改变在线状态
        /// </summary>
        public Receive_0x005C(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(user.TXProtocol.SessionKey);
        }
    }
}