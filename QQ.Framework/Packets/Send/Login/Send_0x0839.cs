using QQ.Framework.Packets.PCTLV;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Login
{
    public class Send_0X0839 : SendPacket
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="user"></param>
        /// <param name="type">包类型</param>
        public Send_0X0839(QQUser user)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = User.TXProtocol.BufDhShareKey;
            Command = QQCommand.Login0X0839;
        }


        protected override void PutHeader()
        {
            base.PutHeader();
            Writer.Write(new byte[] {0x03, 0x00, 0x00});
            Writer.Write(User.TXProtocol.DwClientType);
            Writer.Write(User.TXProtocol.DwPubNo);
            Writer.BeWrite(0);
            Writer.Write(new byte[] {0x00, 0x30, 0x00, 0x3a});
            Writer.WriteKey(User.TXProtocol.BufSigSession);
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            BodyWriter.Write(new TLV050C().Get_Tlv(User));
        }
    }
}