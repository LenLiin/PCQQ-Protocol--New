namespace QQ.Framework.Packets.Receive.Message
{
    /// <summary>
    /// </summary>
    public class Receive_0x0388 : ReceivePacket
    {
        /// <summary>
        /// </summary>
        public Receive_0x0388(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(user.TXProtocol.SessionKey);
            reader.ReadBytes(4);
        }
    }
}