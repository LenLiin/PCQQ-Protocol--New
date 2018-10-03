using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Interactive
{
    public class Send_0X00A8 : SendPacket
    {
        public Send_0X00A8(QQUser user, long addQQ, AddFriendType addType, string message)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Interactive0X00Ae;
            AddQQ = addQQ;
            AddType = addType;
            Message = Encoding.UTF8.GetBytes(message);
        }

        public long AddQQ { get; set; }
        public AddFriendType AddType { get; set; }
        private byte[] Message { get; }

        protected override void PutHeader()
        {
            base.PutHeader();
            SendPACKET_FIX();
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            if (AddType == AddFriendType.AddFriend)
            {
                BodyWriter.Write(new byte[]
                {
                    0x00
                });
                BodyWriter.BeWrite(AddQQ);
                if (User.AddFriend0018Value.Length > 0)
                {
                    BodyWriter.BeWrite((ushort) User.AddFriend0018Value.Length);
                    BodyWriter.Write(User.AddFriend0018Value);
                    BodyWriter.BeWrite((ushort) User.AddFriend0020Value.Length);
                    BodyWriter.Write(User.AddFriend0020Value);
                }
                else
                {
                    BodyWriter.Write(new byte[]
                    {
                        0x00, 0x00
                    });
                    BodyWriter.BeWrite((ushort) User.AddFriend0020Value.Length);
                    BodyWriter.Write(User.AddFriend0020Value);
                }

                BodyWriter.Write(new byte[] {0x10, 0x00});
            }
            else
            {
                BodyWriter.Write(new byte[]
                {
                    0x02
                });
                BodyWriter.BeWrite(AddQQ);
                BodyWriter.Write(new byte[]
                {
                    0x00, 0x00
                });
                BodyWriter.BeWrite((ushort) User.AddFriend0020Value.Length);
                BodyWriter.Write(User.AddFriend0020Value);
                BodyWriter.Write(new byte[] {0x10, 0x00});
                BodyWriter.BeWrite((ushort) Message.Length);
                BodyWriter.Write(Message);
            }

            BodyWriter.Write(new byte[]
            {
                0x01, 0x00, 0x00, 0x0F, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x00
            });
        }
    }
}