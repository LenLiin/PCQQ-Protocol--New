using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Receive.Login
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class Receive_0x00BA : ReceivePacket
    {
        public byte[] VerifyCode { get; set; }
        public byte VerifyCommand { get; set; } = 0x01;
        public byte Status { get; set; }
        public byte VerifyType { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public Receive_0x00BA(ByteBuffer byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_PACKET_00BA_Key)
        {
        }
        protected override void ParseBody(ByteBuffer byteBuffer)
        {
            //密文
            byte[] CipherText = byteBuffer.ToByteArray();
            //明文
            bodyDecrypted = QQTea.Decrypt(CipherText, byteBuffer.Position, CipherText.Length - byteBuffer.Position - 1, _secretKey);
            //提取数据
            ByteBuffer buf = new ByteBuffer(bodyDecrypted);
            VerifyType = buf.Get();
            buf.GetChar();
            Status = buf.Get();
            buf.GetByteArray(4);
            user.QQ_PACKET_00BAVerifyToken = buf.GetByteArray(buf.GetChar());
            VerifyCode = buf.GetByteArray(buf.GetChar());
            VerifyCommand = buf.Get();
            if(VerifyCommand==0x00)
                VerifyCommand = buf.Get();
            buf.Get();
            if (user.QQ_PACKET_00BAVerifyCode?.Length == 0 || user.QQ_PACKET_00BAVerifyCode == null)
            {
                user.QQ_PACKET_00BAVerifyCode = VerifyCode;
            }
            else
            {
                byte[] resultArr = new byte[user.QQ_PACKET_00BAVerifyCode.Length + VerifyCode.Length];
                user.QQ_PACKET_00BAVerifyCode.CopyTo(resultArr, 0);
                VerifyCode.CopyTo(resultArr, user.QQ_PACKET_00BAVerifyCode.Length);
                user.QQ_PACKET_00BAVerifyCode = resultArr;

            }
            user.QQ_PACKET_00BAToken = buf.GetByteArray(buf.GetChar());
            buf.GetByteArray(buf.GetChar());
        }
    }
}
