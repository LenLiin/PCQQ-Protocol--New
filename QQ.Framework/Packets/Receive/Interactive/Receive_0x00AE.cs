namespace QQ.Framework.Packets.Receive.Interactive
{
    public class Receive_0x00AE : ReceivePacket
    {
        /// <summary>
        ///     
        /// </summary>
        public Receive_0x00AE(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_SessionKey)
        {
        }
        public AddFriendType addFriendType { get; set; }
        protected override void ParseBody()
        {
            Decrypt(user.QQ_SessionKey);
            reader.ReadBytes(2);
            addFriendType = (AddFriendType)reader.Read();
            reader.ReadBytes(3);
            user.AddFriend_0020Value = reader.ReadBytes(32);
        }
    }
}