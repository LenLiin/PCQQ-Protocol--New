namespace QQ.Framework.Packets.Send.Message
{
    public class Send_0X01C0 : SendPacket
    {
        /// <summary>
        ///     好友QQ
        /// </summary>
        private readonly byte[] _toQQ;

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="toQQ"></param>
        public Send_0X01C0(QQUser user, byte[] toQQ)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Message0X01C0;
            _toQQ = toQQ;
        }

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
            BodyWriter.Write((byte) 0x01);
            BodyWriter.Write(_toQQ);
        }
    }
}