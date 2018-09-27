using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Receive.Login
{
    /// <summary>
    ///     验证码
    /// </summary>
    public class Receive_0X00Ba : ReceivePacket
    {
        /// <summary>
        ///     验证码
        /// </summary>
        public Receive_0X00Ba(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.QQPacket00BaKey)
        {
        }

        public byte[] VerifyCode { get; set; }
        public byte VerifyCommand { get; set; } = 0x01;
        public byte Status { get; set; }
        public byte VerifyType { get; set; }

        protected override void ParseBody()
        {
            Decrypt(SecretKey);
            VerifyType = Reader.ReadByte();
            Reader.BeReadChar();
            Status = Reader.ReadByte();
            Reader.ReadBytes(4);
            User.TXProtocol.BufSigPic = Reader.ReadBytes(Reader.BeReadChar());
            if (VerifyType == 0x13)
            {
                VerifyCode = Reader.ReadBytes(Reader.BeReadChar());
                VerifyCommand = Reader.ReadByte();
                if (VerifyCommand == 0x00)
                {
                    VerifyCommand = Reader.ReadByte();
                }
                else
                {
                    Reader.ReadByte();
                }

                if (User.QQPacket00BaVerifyCode?.Length == 0 || User.QQPacket00BaVerifyCode == null)
                {
                    User.QQPacket00BaVerifyCode = VerifyCode;
                }
                else
                {
                    var resultArr = new byte[User.QQPacket00BaVerifyCode.Length + VerifyCode.Length];
                    User.QQPacket00BaVerifyCode.CopyTo(resultArr, 0);
                    VerifyCode.CopyTo(resultArr, User.QQPacket00BaVerifyCode.Length);
                    User.QQPacket00BaVerifyCode = resultArr;
                }

                User.TXProtocol.PngToken = Reader.ReadBytes(Reader.BeReadChar());
                Reader.ReadBytes(Reader.BeReadChar());
            }
        }
    }
}