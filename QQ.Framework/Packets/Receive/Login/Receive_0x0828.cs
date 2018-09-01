using System;

namespace QQ.Framework.Packets.Receive.Login
{
    public class Receive_0x0828 : ReceivePacket
    {
        public Receive_0x0828(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_0828_rec_decr_key)
        {
        }

        public byte DataHead { get; set; }

        protected override void ParseBody()
        {
            Decrypt(user.QQ_0828_rec_decr_key);
            if (GetPacketLength() == 407)
            {
                reader.ReadBytes(15);
                user.QQ_SessionKey = reader.ReadBytes(0x10);
            }
            else if (GetPacketLength() == 439 || GetPacketLength() == 527)
            {
                reader.ReadBytes(63);
                user.QQ_SessionKey = reader.ReadBytes(0x10);
            }
            else
            {
                throw new Exception("登录失败");
            }
        }
    }
}