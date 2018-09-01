using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Receive.Login
{
    /// <summary>
    ///     验证码
    /// </summary>
    public class Receive_0x00BA : ReceivePacket
    {
        /// <summary>
        ///     验证码
        /// </summary>
        public Receive_0x00BA(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_PACKET_00BA_Key)
        {
        }

        public byte[] VerifyCode { get; set; }
        public byte VerifyCommand { get; set; } = 0x01;
        public byte Status { get; set; }
        public byte VerifyType { get; set; }

        protected override void ParseBody()
        {
            Decrypt(_secretKey);
            VerifyType = reader.ReadByte();
            reader.BEReadChar();
            Status = reader.ReadByte();
            reader.ReadBytes(4);
            user.QQ_PACKET_00BAVerifyToken = reader.ReadBytes(reader.BEReadChar());
            VerifyCode = reader.ReadBytes(reader.BEReadChar());
            VerifyCommand = reader.ReadByte();
            if (VerifyCommand == 0x00)
            {
                VerifyCommand = reader.ReadByte();
            }

            reader.ReadByte();
            if (user.QQ_PACKET_00BAVerifyCode?.Length == 0 || user.QQ_PACKET_00BAVerifyCode == null)
            {
                user.QQ_PACKET_00BAVerifyCode = VerifyCode;
            }
            else
            {
                var resultArr = new byte[user.QQ_PACKET_00BAVerifyCode.Length + VerifyCode.Length];
                user.QQ_PACKET_00BAVerifyCode.CopyTo(resultArr, 0);
                VerifyCode.CopyTo(resultArr, user.QQ_PACKET_00BAVerifyCode.Length);
                user.QQ_PACKET_00BAVerifyCode = resultArr;
            }

            user.QQ_PACKET_00BAToken = reader.ReadBytes(reader.BEReadChar());
            reader.ReadBytes(reader.BEReadChar());
        }
    }
}