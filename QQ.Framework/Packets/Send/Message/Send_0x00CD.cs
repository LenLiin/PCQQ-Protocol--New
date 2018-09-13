using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Message
{
    /// <summary>
    ///     发送好友消息
    /// </summary>
    public class Send_0x00CD : SendPacket
    {
        public Send_0x00CD(QQUser User, byte[] Message)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Message0x00CD;
            _message = Message;
        }

        /// <summary>
        ///     消息类型
        /// </summary>
        public MessageType _messageType { get; set; }

        private byte[] _message { get; }

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
            bodyWriter.Write(_message);
        }

        public static List<Send_0x00CD> SendLongMessage(QQUser User, Richtext Message, long ToQQ)
        {
            throw new NotImplementedException();
        }

        public static List<byte[]> ConstructMessage(QQUser user, Richtext message, long toQQ)
        {
            var dateTime = Util.GetTimeSeconds(DateTime.Now);
            var md5 = user.QQ_SessionKey;
            var ret = new List<byte[]>();
            var writer = new BinaryWriter(new MemoryStream());
            // FIXME: 使用正确的_packetCount和_packetIndex进行分段
            byte _packetCount = 1, _packetIndex = 0;

            void Init()
            {
                writer.BEWrite(user.QQ);
                writer.BEWrite(toQQ);
                writer.Write(new byte[]
                {
                    0x00, 0x00, 0x00, 0x0D, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00,
                    0x01, 0x01
                });
                writer.Write(new byte[] {0x36, 0x43});
                writer.BEWrite(user.QQ);
                writer.BEWrite(toQQ);
                writer.Write(md5);
                writer.Write(new byte[] {0x00, 0x0B});
                writer.Write(Util.RandomKey(2));
                writer.BEWrite(dateTime);
                writer.Write(new byte[]
                {
                    0x02, 0x34, 0x00, 0x00, 0x00, 0x00, _packetCount, _packetIndex, 0x00, 0x00, 0x01, 0x4D,
                    0x53, 0x47,
                    0x00, 0x00, 0x00, 0x00, 0x00
                });
                writer.BEWrite(dateTime);
                writer.Write(Util.RandomKey(4));
                writer.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                writer.Write(new byte[] {0x00, 0x06});
                writer.Write(new byte[] {0xE5, 0xAE, 0x8B, 0xE4, 0xBD, 0x93});
                writer.Write(new byte[] {0x00, 0x00});
            }

            foreach (var snippet in message.Snippets)
            {
                switch (snippet.Type)
                {
                    case MessageType.Xml:
                    {
                        var compressMsg = GZipByteArray.CompressBytes(snippet.Content);
                        writer.BEWrite(user.QQ);
                        writer.BEWrite(toQQ);
                        writer.Write(new byte[] {0x00, 0x00, 0x00, 0x08, 0x00, 0x01, 0x00, 0x04});
                        writer.Write(new byte[] {0x00, 0x00, 0x00, 0x00});
                        writer.Write(new byte[] {0x37, 0x0F});
                        writer.BEWrite(user.QQ);
                        writer.BEWrite(toQQ);
                        writer.Write(md5);
                        writer.Write(new byte[] {0x00, 0x0B});
                        writer.Write(Util.RandomKey(2));
                        writer.BEWrite(dateTime);
                        writer.Write(new byte[]
                        {
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, _packetCount, _packetIndex, 0x00, 0x00, 0x01, 0x4D,
                            0x53, 0x47,
                            0x00, 0x00, 0x00, 0x00, 0x00
                        });
                        writer.BEWrite(dateTime);
                        writer.Write(Util.RandomKey(4));
                        writer.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                        writer.Write(new byte[] {0x00, 0x0C});
                        writer.Write(
                            new byte[] {0xE5, 0xBE, 0xAE, 0xE8, 0xBD, 0xAF, 0xE9, 0x9B, 0x85, 0xE9, 0xBB, 0x91});
                        writer.Write(new byte[] {0x00, 0x00, 0x14});
                        writer.BEWrite((ushort) (compressMsg.Length + 11));
                        writer.Write((byte) 0x01);
                        writer.BEWrite((ushort) (compressMsg.Length + 1));
                        writer.Write((byte) 0x01);
                        writer.Write(compressMsg);
                        writer.Write(new byte[] {0x02, 0x00, 0x04, 0x00, 0x00, 0x00, 0x4D});
                        ret.Add(writer.BaseStream.ToBytesArray());
                        return ret;
                    }
                    case MessageType.Shake:
                    {
                        writer.BEWrite(user.QQ);
                        writer.BEWrite(toQQ);
                        writer.Write(new byte[] {0x00, 0x00, 0x00, 0x00});
                        writer.Write(new byte[] {0x37, 0x0F});
                        writer.BEWrite(user.QQ);
                        writer.BEWrite(toQQ);
                        writer.Write(Util.RandomKey());
                        writer.Write(new byte[] {0x00, 0xAF});
                        writer.Write(Util.RandomKey(2));
                        writer.BEWrite(dateTime);
                        writer.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00});
                        writer.Write(Util.RandomKey(4));
                        writer.Write(new byte[] {0x00, 0x00, 0x00, 0x00});
                        ret.Add(writer.BaseStream.ToBytesArray());
                        return ret;
                    }
                    case MessageType.Normal:
                    {
                        if (writer.BaseStream.Position + snippet.Length > 699)
                        {
                            ret.Add(writer.BaseStream.ToBytesArray());
                            writer = new BinaryWriter(new MemoryStream());
                        }

                        if (writer.BaseStream.Position == 0)
                        {
                            Init();
                        }

                        var messageData = Encoding.UTF8.GetBytes(snippet.Content);
                        writer.Write(new byte[] {0x01});
                        writer.BEWrite((ushort) (messageData.Length + 3));
                        writer.Write(new byte[] {0x01});
                        writer.BEWrite((ushort) messageData.Length);
                        writer.Write(messageData);
                        break;
                    }
                    case MessageType.Emoji:
                    {
                        if (writer.BaseStream.Position + snippet.Length > 699)
                        {
                            ret.Add(writer.BaseStream.ToBytesArray());
                            writer = new BinaryWriter(new MemoryStream());
                        }

                        if (writer.BaseStream.Position == 0)
                        {
                            Init();
                        }

                        var faceIndex = byte.Parse(snippet.Content);
                        writer.Write(new byte[] {0x02, 0x00, 0x14, 0x01, 0x00, 0x01});
                        writer.Write(faceIndex);
                        writer.Write(new byte[] {0xFF, 0x00, 0x02, 0x14});
                        writer.Write((byte) (faceIndex + 65));
                        writer.Write(new byte[] {0x0B, 0x00, 0x08, 0x00, 0x01, 0x00, 0x04, 0x52, 0xCC, 0x85, 0x50});
                        break;
                    }
                    default:
                    {
                        throw new NotImplementedException();
                    }
                }

                ret.Add(writer.BaseStream.ToBytesArray());
                writer = new BinaryWriter(new MemoryStream());
            }

            return ret;
        }
    }
}