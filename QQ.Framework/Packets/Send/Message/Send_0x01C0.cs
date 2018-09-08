namespace QQ.Framework.Packets.Send.Message
{
    public class Send_0x01C0 : SendPacket
    {
        /// <summary>
        ///     好友QQ
        /// </summary>
        private readonly byte[] _toQQ;

        /// <summary>
        /// </summary>
        /// <param name="User"></param>
        /// <param name="ToQQ"></param>
        public Send_0x01C0(QQUser User, byte[] ToQQ)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Message0x01C0;
            _toQQ = ToQQ;
        }

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
            bodyWriter.Write((byte) 0x01);
            bodyWriter.Write(_toQQ);
        }
    }
}