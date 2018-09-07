using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Interactive
{
    public class Send_0x00AE : SendPacket
    {
        /// <summary>
        ///     获取添加群或好友的令牌
        /// </summary>
        /// <param name="User"></param>
        /// <param name="AddQQ"></param>
        /// <param name="addType"></param>
        public Send_0x00AE(QQUser User, long AddQQ, AddFriendType addType)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Interactive0x00AE;
            _AddQQ = AddQQ;
            this.addType = addType;
        }

        public long _AddQQ { get; set; }
        public AddFriendType addType { get; set; }

        protected override void PutHeader()
        {
            base.PutHeader();
            writer.Write(user.QQ_PACKET_FIXVER);
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            bodyWriter.Write(new byte[]
            {
                0x01, 0x00
            });
            bodyWriter.Write((byte) addType);
            bodyWriter.BEWrite(_AddQQ);
        }
    }
}