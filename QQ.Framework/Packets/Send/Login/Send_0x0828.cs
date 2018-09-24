using QQ.Framework.Packets.PCTLV;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Login
{
    public class Send_0X0828 : SendPacket
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="user"></param>
        public Send_0X0828(QQUser user)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = User.TXProtocol.BufSessionKey;
            Command = QQCommand.Login0X0828;
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            Writer.Write(new byte[] {0x02, 0x00, 0x00});
            Writer.Write(User.TXProtocol.DwClientType);
            Writer.Write(User.TXProtocol.DwPubNo);
            Writer.Write(new byte[] {0x00, 0x30, 0x00, 0x3a});
            Writer.WriteKey(User.TXProtocol.BufSigSession);
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            BodyWriter.Write(new TLV0007().Get_Tlv(User));
            BodyWriter.Write(new TLV000C().Get_Tlv(User));
            BodyWriter.Write(new TLV0015().Get_Tlv(User));
            BodyWriter.Write(new TLV0036().Get_Tlv(User));
            BodyWriter.Write(new TLV0018().Get_Tlv(User));
            BodyWriter.Write(new TLV001F().Get_Tlv(User));
            BodyWriter.Write(new TLV0105().Get_Tlv(User));
            BodyWriter.Write(new TLV010B().Get_Tlv(User));
            BodyWriter.Write(new TLV002D().Get_Tlv(User));
        }
    }
}