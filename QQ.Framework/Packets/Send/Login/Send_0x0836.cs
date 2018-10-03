using QQ.Framework.Packets.PCTLV;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Login
{
    public class Send_0X0836 : SendPacket
    {
        /// <summary>
        ///     数据包类型默认为第一种
        /// </summary>
        private readonly Login0X0836Type _type;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="user"></param>
        /// <param name="type">包类型</param>
        public Send_0X0836(QQUser user, Login0X0836Type type = Login0X0836Type.Login0X0836622, bool isVerify = false)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = User.TXProtocol.BufDhShareKey;
            _type = type;
            Command = QQCommand.Login0X0836;
            IsVerify = isVerify;
        }

        private bool IsVerify { get; }

        protected override void PutHeader()
        {
            base.PutHeader();
            SendPACKET_FIX();
            Writer.BeWrite(User.TXProtocol.SubVer);
            Writer.BeWrite(User.TXProtocol.EcdhVer);
            Writer.BeWrite((ushort) 0x19);
            Writer.Write(User.TXProtocol.BufDhPublicKey);
            Writer.Write(new byte[] {0x00, 0x00, 0x00, 0x10});
            Writer.Write(User.QQPacket0836Key1);
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            BodyWriter.Write(new TLV0112().Get_Tlv(User));
            BodyWriter.Write(new TLV030F().Get_Tlv(User));
            BodyWriter.Write(new TLV0005().Get_Tlv(User));
            BodyWriter.Write(new TLV0006().Get_Tlv(User));
            BodyWriter.Write(new TLV0015().Get_Tlv(User));
            BodyWriter.Write(new TLV001A().Get_Tlv(User));
            BodyWriter.Write(new TLV0018().Get_Tlv(User));
            BodyWriter.Write(new TLV0103().Get_Tlv(User));
            if (_type == Login0X0836Type.Login0X0836686)
            {
                BodyWriter.Write(new TLV0110().Get_Tlv(User));
                BodyWriter.Write(new TLV0032().Get_Tlv(User));
            }

            BodyWriter.Write(new TLV0312().Get_Tlv(User));
            BodyWriter.Write(new TLV0508().Get_Tlv(User));
            BodyWriter.Write(new TLV0313().Get_Tlv(User));
            BodyWriter.Write(new TLV0102().Get_Tlv(User));
        }
    }

    public enum Login0X0836Type
    {
        Login0X0836622 = 1,
        Login0X0836686 = 2,
        Login0X0836710 = 3
    }
}