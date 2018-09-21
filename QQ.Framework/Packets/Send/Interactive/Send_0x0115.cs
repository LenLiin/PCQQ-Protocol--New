using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Interactive
{
    public class Send_0x0115 : SendPacket
    {
        /// <summary>
        /// </summary>
        /// <param name="User"></param>
        /// <param name="AddQQ"></param>
        public Send_0x0115(QQUser User, long AddQQ)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = User.TXProtocol.SessionKey;
            Command = QQCommand.Interactive0x00AE;
            _AddQQ = AddQQ;
        }

        public long _AddQQ { get; set; }

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
                0x03
            });
            bodyWriter.BEWrite(_AddQQ);
        }
    }
}