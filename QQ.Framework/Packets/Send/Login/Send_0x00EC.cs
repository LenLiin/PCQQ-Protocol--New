namespace QQ.Framework.Packets.Send.Login
{
    /// <summary>
    ///     改变在线状态
    /// </summary>
    public class Send_0x00EC : SendPacket
    {
        private readonly byte _loginStatus = LoginStatus.我在线上;

        /// <summary>
        ///     改变在线状态
        /// </summary>
        public Send_0x00EC(QQUser User, byte loginStatus)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Login0x00EC;
            _loginStatus = loginStatus;
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            writer.Write(user.QQ_PACKET_FIXVER);
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutBody()
        {
            bodyWriter.Write(new byte[] {0x01, 0x00});
            bodyWriter.Write(_loginStatus);
            bodyWriter.Write(new byte[] {0x00, 0x01, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00});
        }
    }
}