using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Data
{
    public class Send_0X0126 : SendPacket
    {
        /// <summary>
        ///     获取昵称
        /// </summary>
        /// <param name="user"></param>
        public Send_0X0126(QQUser user, long toQQ)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Data0X0126;
            ToQQ = toQQ;
        }

        public long ToQQ { get; set; }

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
            BodyWriter.Write(new byte[] {0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03});
            BodyWriter.BeWrite(ToQQ);
            BodyWriter.Write(new byte[] {0x69, 0x8e, 0x7e, 0x44, 0x3c, 0x11, 0xea, 0x7c});
        }
    }
}