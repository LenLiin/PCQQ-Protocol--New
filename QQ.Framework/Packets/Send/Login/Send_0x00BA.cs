using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Login
{
    /// <summary>
    ///     验证码
    /// </summary>
    public class Send_0X00Ba : SendPacket
    {
        /// <summary>
        ///     验证码提交
        /// </summary>
        public Send_0X00Ba(QQUser user, string verifyCode)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = User.QQPacket00BaKey;
            Command = QQCommand.Login0X00Ba;
            VerifyCode = verifyCode;
        }

        private string VerifyCode { get; }

        protected override void PutHeader()
        {
            base.PutHeader();
            SendPACKET_FIX();
            Writer.Write(SecretKey);
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            BodyWriter.Write(new byte[] {0x00, 0x02, 0x00, 0x00, 0x08, 0x04, 0x01, 0xE0});
            BodyWriter.BeWrite(User.TXProtocol.DwSsoVersion);
            BodyWriter.BeWrite(User.TXProtocol.DwServiceId);
            BodyWriter.BeWrite(User.TXProtocol.DwClientVer);
            BodyWriter.Write((byte) 0x00);
            BodyWriter.WriteKey(User.TXProtocol.BufSigClientAddr);
            BodyWriter.Write(new byte[] {0x01, 0x02});
            BodyWriter.WriteKey(User.TXProtocol.BufDhPublicKey);
            if (string.IsNullOrEmpty(VerifyCode))
            {
                BodyWriter.Write(new byte[] {0x13, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00});
                BodyWriter.Write(User.QQPacket00BaSequence);
                if (User.TXProtocol.PngToken == null || User.TXProtocol.PngToken?.Length == 0)
                {
                    BodyWriter.Write((byte) 0x00);
                }
                else
                {
                    BodyWriter.WriteKey(User.TXProtocol.PngToken);
                }
            }
            else
            {
                var verifyCodeBytes = Util.HexStringToByteArray(Util.ConvertStringToHex(VerifyCode));
                BodyWriter.Write(new byte[] {0x14, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00});
                BodyWriter.BeWrite((ushort) verifyCodeBytes.Length);
                BodyWriter.Write(verifyCodeBytes);
                BodyWriter.WriteKey(User.TXProtocol.BufSigPic);
                //输入验证码后清空图片流
                User.QQPacket00BaVerifyCode = new byte[] { };
            }

            BodyWriter.WriteKey(User.QQPacket00BaFixKey);
        }

        protected override void PutTail()
        {
            base.PutTail();
            User.QQPacket00BaSequence++;
        }
    }
}