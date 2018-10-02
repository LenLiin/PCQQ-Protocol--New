namespace QQ.Framework.Packets.Send.Message
{
    public class Send_0X0017 : SendPacket
    {
        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="data">要发送的数据内容</param>
        /// <param name="sequence">序号</param>
        public Send_0X0017(QQUser user, byte[] data, char sequence)
            : base(user)
        {
            Sequence = sequence;
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Message0X0017;
            Data = data;
        }

        private byte[] Data { get; }

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
            BodyWriter.Write(Data);
        }
    }
}