namespace QQ.Framework.Packets.Receive.Message
{
    /// <summary>
    /// 
    /// </summary>
    public class Receive_0x0352 : ReceivePacket
    {
        /// <summary>
        ///     
        /// </summary>
        public Receive_0x0352(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(user.QQ_SessionKey);
            reader.ReadBytes(4);
        }
    }
}