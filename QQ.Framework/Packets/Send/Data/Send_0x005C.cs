using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Data
{
    public class Send_0x005C : SendPacket
    {
        public Send_0x005C(QQUser User)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = User.TXProtocol.SessionKey;
            Command = QQCommand.Data0x005C;
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            Sequence = GetNextSeq();
            writer.Write(user.QQ_PACKET_FIXVER);
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            bodyWriter.Write((byte) 0x88);
            bodyWriter.BEWrite(user.QQ);
            bodyWriter.Write((byte) 0x00);
        }
    }
}