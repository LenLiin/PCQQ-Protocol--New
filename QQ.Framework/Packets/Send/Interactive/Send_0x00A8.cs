using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Interactive
{
    public class Send_0x00A8 : SendPacket
    {
        public Send_0x00A8(QQUser User, long AddQQ, AddFriendType addType, string Message)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Interactive0x00AE;
            _AddQQ = AddQQ;
            this.addType = addType;
            _message = Encoding.UTF8.GetBytes(Message);
        }

        public long _AddQQ { get; set; }
        public AddFriendType addType { get; set; }
        private byte[] _message { get; }

        protected override void PutHeader()
        {
            base.PutHeader();
            writer.Write(user.QQ_PACKET_FIXVER);
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutBody()
        {
            if (addType == AddFriendType.AddFriend)
            {
                bodyWriter.Write(new byte[]
                {
                    0x00
                });
                bodyWriter.BEWrite(_AddQQ);
                if (user.AddFriend_0018Value.Length > 0)
                {
                    bodyWriter.BEWrite((ushort) user.AddFriend_0018Value.Length);
                    bodyWriter.Write(user.AddFriend_0018Value);
                    bodyWriter.BEWrite((ushort) user.AddFriend_0020Value.Length);
                    bodyWriter.Write(user.AddFriend_0020Value);
                }
                else
                {
                    bodyWriter.Write(new byte[]
                    {
                        0x00, 0x00
                    });
                    bodyWriter.BEWrite((ushort) user.AddFriend_0020Value.Length);
                    bodyWriter.Write(user.AddFriend_0020Value);
                }

                bodyWriter.Write(new byte[] {0x10, 0x00});
            }
            else
            {
                bodyWriter.Write(new byte[]
                {
                    0x02
                });
                bodyWriter.BEWrite(_AddQQ);
                bodyWriter.Write(new byte[]
                {
                    0x00, 0x00
                });
                bodyWriter.BEWrite((ushort) user.AddFriend_0020Value.Length);
                bodyWriter.Write(user.AddFriend_0020Value);
                bodyWriter.Write(new byte[] {0x10, 0x00});
                bodyWriter.BEWrite((ushort) _message.Length);
                bodyWriter.Write(_message);
            }

            bodyWriter.Write(new byte[]
            {
                0x01, 0x00, 0x00, 0x0F, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x00
            });
        }
    }
}