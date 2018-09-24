using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Message
{
    /// <summary>
    ///     点赞
    /// </summary>
    public class Send_0X03E3 : SendPacket
    {
        /// <summary>
        ///     好友QQ
        /// </summary>
        private readonly long _toQQ;

        /// <summary>
        ///     点赞
        /// </summary>
        /// <param name="user"></param>
        /// <param name="toQQ">要点赞的QQ</param>
        public Send_0X03E3(QQUser user, long toQQ)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Interactive0X03E3;
            _toQQ = toQQ;
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            Writer.Write(new byte[]
            {
                0x04, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x00, 0x68, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00
            });
        }

        protected override void PutBody()
        {
            var myid = Util.HexStringToByteArray(Util.PB_toLength(User.QQ));
            var id = Util.HexStringToByteArray(Util.PB_toLength(_toQQ));
            BodyWriter.Write(new byte[] {0x00, 0x00, 0x00});
            BodyWriter.BeWrite(myid.Length + 11);
            BodyWriter.Write(new byte[] {0x00, 0x00, 0x00});
            BodyWriter.BeWrite(myid.Length + id.Length + 8);
            BodyWriter.Write(new byte[] {0x08, 0x01});
            BodyWriter.Write(new byte[] {0x12});
            BodyWriter.BeWrite(myid.Length + 7);
            BodyWriter.Write(new byte[] {0x08});
            BodyWriter.Write(myid);
            BodyWriter.Write(new byte[] {0x10, 0xE3, 0x07, 0x98, 0x01, 0x00});
            BodyWriter.Write(new byte[] {0x08, 0xE5, 0x0F});
            BodyWriter.Write(new byte[] {0x10, 0x01});
            BodyWriter.Write(new byte[] {0x22});
            BodyWriter.BeWrite(id.Length + 6);
            BodyWriter.Write(new byte[] {0x58});
            BodyWriter.Write(id);
            BodyWriter.Write(new byte[] {0x60, 0x92, 0x4E});
            BodyWriter.Write(new byte[] {0x68, 0x01});
        }
    }
}