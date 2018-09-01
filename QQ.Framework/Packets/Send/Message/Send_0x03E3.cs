using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Message
{
    /// <summary>
    ///     点赞
    /// </summary>
    public class Send_0x03E3 : SendPacket
    {
        /// <summary>
        ///     好友QQ
        /// </summary>
        private readonly long _toQQ;

        /// <summary>
        ///     点赞
        /// </summary>
        /// <param name="User"></param>
        /// <param name="ToQQ">要点赞的QQ</param>
        public Send_0x03E3(QQUser User, long ToQQ)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Interactive0x03E3;
            _toQQ = ToQQ;
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            writer.Write(new byte[]
            {
                0x04, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x00, 0x68, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00
            });
        }

        protected override void PutBody()
        {
            var myid = Util.HexStringToByteArray(Util.PB_toLength(user.QQ));
            var id = Util.HexStringToByteArray(Util.PB_toLength(_toQQ));
            bodyWriter.Write(new byte[] {0x00, 0x00, 0x00});
            bodyWriter.BEWrite(myid.Length + 11);
            bodyWriter.Write(new byte[] {0x00, 0x00, 0x00});
            bodyWriter.BEWrite(myid.Length + id.Length + 8);
            bodyWriter.Write(new byte[] {0x08, 0x01});
            bodyWriter.Write(new byte[] {0x12});
            bodyWriter.BEWrite(myid.Length + 7);
            bodyWriter.Write(new byte[] {0x08});
            bodyWriter.Write(myid);
            bodyWriter.Write(new byte[] {0x10, 0xE3, 0x07, 0x98, 0x01, 0x00});
            bodyWriter.Write(new byte[] {0x08, 0xE5, 0x0F});
            bodyWriter.Write(new byte[] {0x10, 0x01});
            bodyWriter.Write(new byte[] {0x22});
            bodyWriter.BEWrite(id.Length + 6);
            bodyWriter.Write(new byte[] {0x58});
            bodyWriter.Write(id);
            bodyWriter.Write(new byte[] {0x60, 0x92, 0x4E});
            bodyWriter.Write(new byte[] {0x68, 0x01});
        }
    }
}