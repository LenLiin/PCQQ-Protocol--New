using System;
using System.Collections.Generic;
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
        private readonly byte _packetIndex;

        public List<Send_0X00Cd> Following;
        private readonly byte[] _data;

        public Send_0X00Cd(QQUser user, Richtext message, long toQQ)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Message0X00Cd;
            Message = message;
            _toQQ = toQQ;
            Following = new List<Send_0X00Cd>();
            var data = Util.WriteRichtext(message);
            var count = data.Count;
            var index = 1;
            _data = data[0];
            data.RemoveAt(0);
            foreach (var part in data)
            {
                Following.Add(new Send_0X00Cd(user, part, toQQ, index++, count));
            }
        }

        private Send_0X00Cd(QQUser user, byte[] data, long toQQ, int index, int count) : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Message0X0002;
            _data = data;
            _toQQ = toQQ;
            _packetIndex = (byte) index;
            _packetCount = (byte) count;
        }

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
            switch (Message?.Snippets[0].Type ?? MessageType.Normal)
            {
                case MessageType.At:
                case MessageType.Normal:
                case MessageType.Picture:
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
                    BodyWriter.Write(_data);
                    break;
                }
                case MessageType.Xml:
                {
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
                    BodyWriter.BeWrite(dateTime);
                    BodyWriter.Write(SendXml(GZipByteArray.CompressBytes(Message.Snippets[0].Content)));
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
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}