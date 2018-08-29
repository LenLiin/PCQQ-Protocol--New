using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Receive.Login
{
    public class Receive_0x0836 : ReceivePacket
    {
        public byte[] VerifyCode { get; set; }
        public byte VerifyCommand { get; set; } = 0x01;
        public byte DataHead { get; set; }
        public Receive_0x0836(ByteBuffer byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_PACKET_TgtgtKey)
        {
        }
        protected override void ParseBody(ByteBuffer byteBuffer)
        {
            //密文
            byte[] CipherText = byteBuffer.ToByteArray();
            //明文
            byte[] CipherText2 = QQTea.Decrypt(CipherText, byteBuffer.Position, CipherText.Length - byteBuffer.Position - 1, user.QQ_SHARE_KEY);
            if (CipherText2 == null)
            {
                throw new Exception($"包内容解析出错，抛弃该包: {ToString()}");
            }
            if (GetPacketLength() == 871)
            {
                bodyDecrypted = CipherText2;
                ByteBuffer buf = new ByteBuffer(bodyDecrypted);
                buf.GetByteArray(20);
                user.QQ_PACKET_00BAVerifyToken = buf.GetByteArray(buf.GetChar());
                VerifyCode = buf.GetByteArray(buf.GetChar());
                VerifyCommand = buf.Get();
                if (VerifyCommand == 0x00)
                    VerifyCommand = buf.Get();
                user.QQ_PACKET_00BAVerifyCode = VerifyCode;
                user.QQ_PACKET_00BAToken = buf.GetByteArray(buf.GetChar());
                buf.GetByteArray(buf.GetChar());
            }
            else
            {
                bodyDecrypted = QQTea.Decrypt(CipherText2, _secretKey);
                if (bodyDecrypted == null)
                {
                    throw new Exception($"包内容解析出错，抛弃该包: {ToString()}");
                }
                //提取数据
                ByteBuffer buf = new ByteBuffer(bodyDecrypted);
                DataHead = buf.Get();
                if (GetPacketLength() == 271 || GetPacketLength() == 207)
                {
                    buf.GetChar();
                    user.QQ_PACKET_TgtgtKey = buf.GetByteArray(buf.GetChar());
                    buf.GetChar();
                    user.QQ_tlv_0006_encr = buf.GetByteArray(buf.GetChar());
                    buf.GetByteArray(6);
                    if (GetPacketLength() == 271)
                    {
                        user.QQ_0836Token = buf.GetByteArray(buf.GetChar());
                    }
                    buf.GetChar();
                    buf.GetByteArray(buf.GetChar());
                }
                else if (GetPacketLength() > 700)
                {
                    buf.GetByteArray(6);
                    user.QQ_0828_rec_ecr_key = buf.GetByteArray(0x10);
                    buf.GetChar();
                    user.QQ_0836_038Token = buf.GetByteArray(0x38);
                    buf.GetByteArray(60);
                    var Judge = buf.GetByteArray(2);
                    var MsgLength = 0;
                    if (Util.ToHex(Judge) == "01 07")
                    {
                        MsgLength = 0;
                    }
                    else if (Util.ToHex(Judge) == "00 33")
                    {
                        MsgLength = 28;
                    }
                    else if (Util.ToHex(Judge) == "01 10")
                    {
                        MsgLength = 64;
                    }
                    buf.GetByteArray(28);
                    buf.GetByteArray(MsgLength);
                    user.QQ_0828_rec_decr_key = buf.GetByteArray(0x10);
                    buf.GetChar();
                    user.QQ_0836_088Token = buf.GetByteArray(0x88);
                    buf.GetByteArray(159);
                    user.QQ_ClientKey = buf.GetByteArray(112);
                    buf.GetByteArray(28);
                    var nick_length = buf.Get();
                    user.NickName = Util.ConvertHexToString(Util.ToHex(buf.GetByteArray(nick_length)));
                    user.Gender = buf.Get();
                    buf.GetByteArray(4);
                    user.Age = buf.Get();
                    buf.GetByteArray(10);
                    buf.GetByteArray(0x10);
                }
            }
        }
    }
}
