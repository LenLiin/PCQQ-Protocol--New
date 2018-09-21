using System.IO;
using QQ.Framework.Packets.PCTLV;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Login
{
    public class Send_0x0836 : SendPacket
    {
        /// <summary>
        ///     数据包类型默认为第一种
        /// </summary>
        private readonly Login0x0836Type _type;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="User"></param>
        /// <param name="type">包类型</param>
        public Send_0x0836(QQUser User, Login0x0836Type type = Login0x0836Type.Login0x0836_622, bool isVerify = false)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.TXProtocol.bufDHShareKey;
            _type = type;
            Command = QQCommand.Login0x0836;
            this.isVerify = isVerify;
        }

        private bool isVerify { get; }

        protected override void PutHeader()
        {
            base.PutHeader();
            SendPACKET_FIX();
            writer.BEWrite(user.TXProtocol.SubVer);
            writer.BEWrite(user.TXProtocol.EcdhVer);
            writer.BEWrite((ushort)0x19);
            writer.Write(user.TXProtocol.bufDHPublicKey);
            writer.Write(new byte[] {0x00, 0x00, 0x00, 0x10});
            writer.Write(user.QQ_PACKET_0836_KEY1);
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            bodyWriter.Write(new TLV_0112().Get_Tlv(user));
            bodyWriter.Write(new TLV_030F().Get_Tlv(user));
            bodyWriter.Write(new TLV_0005().Get_Tlv(user));
            bodyWriter.Write(new TLV_0006().Get_Tlv(user));
            bodyWriter.Write(new TLV_0015().Get_Tlv(user));
            bodyWriter.Write(new TLV_001A().Get_Tlv(user));
            bodyWriter.Write(new TLV_0018().Get_Tlv(user));
            bodyWriter.Write(new TLV_0103().Get_Tlv(user));
            if (_type == Login0x0836Type.Login0x0836_686)
            {
                bodyWriter.Write(new TLV_0110().Get_Tlv(user));
                bodyWriter.Write(new TLV_0032().Get_Tlv(user));
            }
            bodyWriter.Write(new TLV_0312().Get_Tlv(user));
            bodyWriter.Write(new TLV_0508().Get_Tlv(user));
            bodyWriter.Write(new TLV_0313().Get_Tlv(user));
            bodyWriter.Write(new TLV_0102().Get_Tlv(user));
        }
    }

    public enum Login0x0836Type
    {
        Login0x0836_622 = 1,
        Login0x0836_686 = 2,
        Login0x0836_710 = 3
    }
}