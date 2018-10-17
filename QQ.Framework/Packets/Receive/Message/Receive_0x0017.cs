using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Receive.Message
{
    public class Receive_0X0017 : ReceivePacket
    {
        /// <summary>
        ///     群消息
        /// </summary>
        public Receive_0X0017(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        /// <summary>
        ///     消息来源QQ
        /// </summary>
        public long FromQQ { get; set; }

        /// <summary>
        ///     群号
        /// </summary>
        public long Group { get; set; }

        /// <summary>
        ///     消息
        /// </summary>
        public Richtext Message { get; set; } = new Richtext();

        /// <summary>
        ///     字体
        /// </summary>
        public byte[] Font { get; set; }

        /// <summary>
        ///     接收时间
        /// </summary>
        public byte[] ReceiveTime { get; set; }

        /// <summary>
        ///     发送时间
        /// </summary>
        public byte[] SendTime { get; set; }

        /// <summary>
        ///     消息 id
        /// </summary>
        public byte[] MessageId { get; set; }

        /// <summary>
        ///     消息索引
        /// </summary>
        public byte[] MessageIndex { get; set; }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
            Reader.ReadBytes(4); // 疑似群号
            Reader.ReadBytes(4); //自己的QQ
            Reader.ReadBytes(10);
            var type = Reader.BeReadUInt16();
            Reader.ReadBytes(Reader.BeReadInt32());
            Group = Reader.BeReadLong32(); //群号
            var flag = Reader.ReadByte();
            // TODO: 把系统消息接口分出来
            switch (type)
            {
                case 0x52 when flag == 0x01: // 群消息、被拉进/踢出群
                {
                    FromQQ = Reader.BeReadLong32(); //发消息人的QQ
                    MessageIndex = Reader.ReadBytes(4); //姑且叫消息索引吧
                    ReceiveTime = Reader.ReadBytes(4); //接收时间  
                    Reader.ReadBytes(24);
                    SendTime = Reader.ReadBytes(4); //发送时间 
                    MessageId = Reader.ReadBytes(4); //消息 id
                    Reader.ReadBytes(8);
                    Font = Reader.ReadBytes(Reader.BeReadUInt16()); //字体
                    Reader.ReadByte();
                    Reader.ReadByte();
                    Message = Reader.ReadRichtext();
                    break;
                }
                case 0x21:
                case 0x22:
                {
                    Reader.ReadBytes(5);
                    FromQQ = Reader.BeReadLong32(); // 邀请人/踢人QQ
                    break;
                }
                case 0x2C:
                {
                    // 群管理变更
                    break;
                }
                default:
                {
                    if (flag == 0x0C) // 被塞口球
                    {
                        Reader.ReadByte(); // 01?
                        var muter = Reader.BeReadLong32();
                        Reader.ReadBytes(4); // 疑似时间？
                        Reader.ReadBytes(2); // 00 01?
                        var victim = Reader.BeReadLong32();
                        var time = Reader.BeReadLong32();
                        Message = new TextSnippet("", MessageType.Mute, ("Muter", muter), ("Victim", victim),
                            ("Time", time));
                    }

                    break;
                }
            }
        }
    }
}