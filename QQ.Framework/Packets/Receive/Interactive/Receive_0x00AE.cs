namespace QQ.Framework.Packets.Receive.Interactive
{
    public class Receive_0X00Ae : ReceivePacket
    {
        /// <summary>
        /// </summary>
        public Receive_0X00Ae(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        public AddFriendType AddFriendType { get; set; }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
            Reader.ReadBytes(2);
            AddFriendType = (AddFriendType) Reader.Read();
            Reader.ReadBytes(3);
            User.AddFriend0020Value = Reader.ReadBytes(32);
        }
    }
}