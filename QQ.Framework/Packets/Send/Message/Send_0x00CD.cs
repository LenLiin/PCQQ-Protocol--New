using System;
using System.Collections.Generic;
using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Message
{
    /// <summary>
    ///     发送好友消息
    /// </summary>
    public class Send_0x00CD : SendPacket
    {
        /// <summary>
        ///     好友QQ
        /// </summary>
        private readonly long _toQQ;

        private readonly byte _packetCount = 1;
        private byte _packetIndex;

        public Send_0x00CD(QQUser User, Richtext Message, MessageType messageType, long ToQQ)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Message0x00CD;
            _message = Message;
            _messageType = messageType;
            _toQQ = ToQQ;
        }

        /// <summary>
        ///     消息类型
        /// </summary>
        public MessageType _messageType { get; set; }

        private Richtext _message { get; }

        protected override void PutHeader()
        {
            base.PutHeader();
            writer.Write(user.QQ_PACKET_FIXVER);
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            var _DateTime = Util.GetTimeSeconds(DateTime.Now);
            var _Md5 = user.QQ_SessionKey;
            foreach (var snippet in _message.Snippets)
            {
                switch (snippet.Type)
                {
                    case MessageType.Xml:
                    {
                        var compressMsg = GZipByteArray.CompressBytes(snippet.Content);
                        bodyWriter.BEWrite(user.QQ);
                        bodyWriter.BEWrite(_toQQ);
                        bodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x08, 0x00, 0x01, 0x00, 0x04});
                        bodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00});
                        bodyWriter.Write(new byte[] {0x37, 0x0F});
                        bodyWriter.BEWrite(user.QQ);
                        bodyWriter.BEWrite(_toQQ);
                        bodyWriter.Write(_Md5);
                        bodyWriter.Write(new byte[] {0x00, 0x0B});
                        bodyWriter.Write(Util.RandomKey(2));
                        bodyWriter.BEWrite(_DateTime);
                        bodyWriter.Write(new byte[]
                        {
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, _packetCount, _packetIndex, 0x00, 0x00, 0x01, 0x4D,
                            0x53, 0x47,
                            0x00, 0x00, 0x00, 0x00, 0x00
                        });
                        bodyWriter.Write(SendXML(_DateTime, compressMsg));
                        break;
                    }
                    case MessageType.Shake:
                    {
                        bodyWriter.BEWrite(user.QQ);
                        bodyWriter.BEWrite(_toQQ);
                        bodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00});
                        bodyWriter.Write(new byte[] {0x37, 0x0F});
                        bodyWriter.BEWrite(user.QQ);
                        bodyWriter.BEWrite(_toQQ);
                        bodyWriter.Write(Util.RandomKey());
                        bodyWriter.Write(new byte[] {0x00, 0xAF});
                        bodyWriter.Write(Util.RandomKey(2));
                        bodyWriter.BEWrite(_DateTime);
                        bodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00});
                        bodyWriter.Write(Util.RandomKey(4));
                        bodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00});
                        break;
                    }
                    case MessageType.Normal:
                    {
                        bodyWriter.BEWrite(user.QQ);
                        bodyWriter.BEWrite(_toQQ);
                        bodyWriter.Write(new byte[]
                        {
                            0x00, 0x00, 0x00, 0x0D, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00,
                            0x01, 0x01
                        });
                        bodyWriter.Write(new byte[] {0x36, 0x43});
                        bodyWriter.BEWrite(user.QQ);
                        bodyWriter.BEWrite(_toQQ);
                        bodyWriter.Write(_Md5);
                        bodyWriter.Write(new byte[] {0x00, 0x0B});
                        bodyWriter.Write(Util.RandomKey(2));
                        bodyWriter.BEWrite(_DateTime);
                        bodyWriter.Write(new byte[]
                        {
                            0x02, 0x34, 0x00, 0x00, 0x00, 0x00, _packetCount, _packetIndex, 0x00, 0x00, 0x01, 0x4D,
                            0x53, 0x47,
                            0x00, 0x00, 0x00, 0x00, 0x00
                        });
                        bodyWriter.BEWrite(_DateTime);
                        bodyWriter.Write(Util.RandomKey(4));
                        bodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                        bodyWriter.Write(new byte[] {0x00, 0x06});
                        bodyWriter.Write(new byte[] {0xE5, 0xAE, 0x8B, 0xE4, 0xBD, 0x93});
                        bodyWriter.Write(new byte[] {0x00, 0x00});
                        var _messageData = Encoding.UTF8.GetBytes(snippet.Content);
                        ConstructMessage(bodyWriter, _messageData);
                        break;
                    }
                    case MessageType.Emoji:
                    {
                        bodyWriter.BEWrite(user.QQ);
                        bodyWriter.BEWrite(_toQQ);
                        bodyWriter.Write(new byte[]
                        {
                            0x00, 0x00, 0x00, 0x0D, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00,
                            0x01, 0x01
                        });
                        bodyWriter.Write(new byte[] {0x36, 0x43});
                        bodyWriter.BEWrite(user.QQ);
                        bodyWriter.BEWrite(_toQQ);
                        bodyWriter.Write(_Md5);
                        bodyWriter.Write(new byte[] {0x00, 0x0B});
                        bodyWriter.Write(Util.RandomKey(2));
                        bodyWriter.BEWrite(_DateTime);
                        bodyWriter.Write(new byte[]
                        {
                            0x02, 0x34, 0x00, 0x00, 0x00, 0x00, _packetCount, _packetIndex, 0x00, 0x00, 0x01, 0x4D,
                            0x53, 0x47,
                            0x00, 0x00, 0x00, 0x00, 0x00
                        });
                        bodyWriter.BEWrite(_DateTime);
                        bodyWriter.Write(Util.RandomKey(4));
                        bodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                        bodyWriter.Write(new byte[] {0x00, 0x06});
                        bodyWriter.Write(new byte[] {0xE5, 0xAE, 0x8B, 0xE4, 0xBD, 0x93});
                        bodyWriter.Write(new byte[] {0x00, 0x00});
                        var MessageData = ConstructMessage(snippet.Content);
                        if (MessageData.Length != 0)
                        {
                            bodyWriter.Write(MessageData);
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

        public static List<Send_0x00CD> SendLongMessage(QQUser User, Richtext Message, long ToQQ)
        {
            throw new NotImplementedException();
        }
    }
}