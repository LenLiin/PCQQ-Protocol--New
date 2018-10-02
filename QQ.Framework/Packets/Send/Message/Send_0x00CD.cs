using System;
using System.Collections.Generic;
using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Message
{
    /// <summary>
    ///     发送好友消息
    /// </summary>
    public class Send_0X00Cd : SendPacket
    {
        /// <summary>
        ///     好友QQ
        /// </summary>
        private readonly long _toQQ;

        private readonly byte _packetCount = 1;
        private byte _packetIndex;

        public Send_0X00Cd(QQUser user, Richtext message, MessageType messageType, long toQQ)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Message0X00Cd;
            Message = message;
            MessageType = messageType;
            _toQQ = toQQ;
        }

        /// <summary>
        ///     消息类型
        /// </summary>
        public MessageType MessageType { get; set; }

        private Richtext Message { get; }

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
            var dateTime = Util.GetTimeSeconds(DateTime.Now);
            var md5 = User.TXProtocol.SessionKey;
            foreach (var snippet in Message.Snippets)
            {
                switch (snippet.Type)
                {
                    case MessageType.Xml:
                    {
                        var compressMsg = GZipByteArray.CompressBytes(snippet.Content);
                        BodyWriter.BeWrite(User.QQ);
                        BodyWriter.BeWrite(_toQQ);
                        BodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x08, 0x00, 0x01, 0x00, 0x04});
                        BodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00});
                        BodyWriter.Write(new byte[] {0x37, 0x0F});
                        BodyWriter.BeWrite(User.QQ);
                        BodyWriter.BeWrite(_toQQ);
                        BodyWriter.Write(md5);
                        BodyWriter.Write(new byte[] {0x00, 0x0B});
                        BodyWriter.Write(Util.RandomKey(2));
                        BodyWriter.BeWrite(dateTime);
                        BodyWriter.Write(new byte[]
                        {
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, _packetCount, _packetIndex, 0x00, 0x00, 0x01, 0x4D,
                            0x53, 0x47,
                            0x00, 0x00, 0x00, 0x00, 0x00
                        });
                        BodyWriter.Write(SendXml(dateTime, compressMsg));
                        break;
                    }
                    case MessageType.Shake:
                    {
                        BodyWriter.BeWrite(User.QQ);
                        BodyWriter.BeWrite(_toQQ);
                        BodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00});
                        BodyWriter.Write(new byte[] {0x37, 0x0F});
                        BodyWriter.BeWrite(User.QQ);
                        BodyWriter.BeWrite(_toQQ);
                        BodyWriter.Write(Util.RandomKey());
                        BodyWriter.Write(new byte[] {0x00, 0xAF});
                        BodyWriter.Write(Util.RandomKey(2));
                        BodyWriter.BeWrite(dateTime);
                        BodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00});
                        BodyWriter.Write(Util.RandomKey(4));
                        BodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00});
                        break;
                    }
                    case MessageType.Normal:
                    {
                        BodyWriter.BeWrite(User.QQ);
                        BodyWriter.BeWrite(_toQQ);
                        BodyWriter.Write(new byte[]
                        {
                            0x00, 0x00, 0x00, 0x0D, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00,
                            0x01, 0x01
                        });
                        BodyWriter.Write(new byte[] {0x36, 0x43});
                        BodyWriter.BeWrite(User.QQ);
                        BodyWriter.BeWrite(_toQQ);
                        BodyWriter.Write(md5);
                        BodyWriter.Write(new byte[] {0x00, 0x0B});
                        BodyWriter.Write(Util.RandomKey(2));
                        BodyWriter.BeWrite(dateTime);
                        BodyWriter.Write(new byte[]
                        {
                            0x02, 0x34, 0x00, 0x00, 0x00, 0x00, _packetCount, _packetIndex, 0x00, 0x00, 0x01, 0x4D,
                            0x53, 0x47,
                            0x00, 0x00, 0x00, 0x00, 0x00
                        });
                        BodyWriter.BeWrite(dateTime);
                        BodyWriter.Write(Util.RandomKey(4));
                        BodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                        BodyWriter.Write(new byte[] {0x00, 0x06});
                        BodyWriter.Write(new byte[] {0xE5, 0xAE, 0x8B, 0xE4, 0xBD, 0x93});
                        BodyWriter.Write(new byte[] {0x00, 0x00});
                        var messageData = Encoding.UTF8.GetBytes(snippet.Content);
                        ConstructMessage(BodyWriter, messageData);
                        break;
                    }
                    case MessageType.Emoji:
                    {
                        BodyWriter.BeWrite(User.QQ);
                        BodyWriter.BeWrite(_toQQ);
                        BodyWriter.Write(new byte[]
                        {
                            0x00, 0x00, 0x00, 0x0D, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00,
                            0x01, 0x01
                        });
                        BodyWriter.Write(new byte[] {0x36, 0x43});
                        BodyWriter.BeWrite(User.QQ);
                        BodyWriter.BeWrite(_toQQ);
                        BodyWriter.Write(md5);
                        BodyWriter.Write(new byte[] {0x00, 0x0B});
                        BodyWriter.Write(Util.RandomKey(2));
                        BodyWriter.BeWrite(dateTime);
                        BodyWriter.Write(new byte[]
                        {
                            0x02, 0x34, 0x00, 0x00, 0x00, 0x00, _packetCount, _packetIndex, 0x00, 0x00, 0x01, 0x4D,
                            0x53, 0x47,
                            0x00, 0x00, 0x00, 0x00, 0x00
                        });
                        BodyWriter.BeWrite(dateTime);
                        BodyWriter.Write(Util.RandomKey(4));
                        BodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                        BodyWriter.Write(new byte[] {0x00, 0x06});
                        BodyWriter.Write(new byte[] {0xE5, 0xAE, 0x8B, 0xE4, 0xBD, 0x93});
                        BodyWriter.Write(new byte[] {0x00, 0x00});
                        var messageData = ConstructMessage(snippet.Content);
                        if (messageData.Length != 0)
                        {
                            BodyWriter.Write(messageData);
                        }

                        break;
                    }
                    default:
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

        public static List<Send_0X00Cd> SendLongMessage(QQUser user, Richtext message, long toQQ)
        {
            throw new NotImplementedException();
        }
    }
}