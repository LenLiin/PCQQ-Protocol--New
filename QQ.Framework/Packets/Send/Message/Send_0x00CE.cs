namespace QQ.Framework.Packets.Send.Message
{
    public class Send_0x00CE : SendPacket
    {
        /// <summary>
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Data">要发送的数据内容</param>
        /// <param name="_sequence">序号</param>
        public Send_0x00CE(QQUser User, byte[] Data, char _sequence)
            : base(User)
        {
            Sequence = _sequence;
            _secretKey = User.TXProtocol.SessionKey;
            Command = QQCommand.Message0x00CE;
            _data = Data;
        }

        private byte[] _data { get; }

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
            bodyWriter.Write(_data);
        }
    }
}