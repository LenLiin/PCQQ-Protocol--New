using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Interactive
{
    public class Send_0X00Ae : SendPacket
    {
        /// <summary>
        ///     获取添加群或好友的令牌
        /// </summary>
        /// <param name="user"></param>
        /// <param name="addQQ"></param>
        /// <param name="addType"></param>
        public Send_0X00Ae(QQUser user, long addQQ, AddFriendType addType)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Interactive0X00Ae;
            AddQQ = addQQ;
            AddType = addType;
        }

        public long AddQQ { get; set; }
        public AddFriendType AddType { get; set; }

        protected override void PutHeader()
        {
            base.PutHeader();
            SendPACKET_FIX();
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            BodyWriter.Write(new byte[]
            {
                0x01, 0x00
            });
            BodyWriter.Write((byte) AddType);
            BodyWriter.BeWrite(AddQQ);
        }
    }
}