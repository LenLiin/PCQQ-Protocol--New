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
        /// <summary>
        ///     好友QQ
        /// </summary>
        private readonly long _toQQ;

        private byte _packetCount = 1;
        private byte _packetIndex;

        public Send_0x00CD(QQUser User, string Message, MessageType messageType, long ToQQ)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Message0x00CD;
            _message = Encoding.UTF8.GetBytes(Message);
            _messageType = messageType;
            _toQQ = ToQQ;
        }

        /// <summary>
        ///     消息类型
        /// </summary>
        public MessageType _messageType { get; set; }

        private byte[] _message { get; set; }

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
            var _DateTime = Util.GetTimeSeconds(DateTime.Now);
            var _Md5 = user.QQ_SessionKey;
            if (_messageType.HasFlag(MessageType.Xml))
            {
                var compressMsg = GZipByteArray.CompressBytes(Encoding.UTF8.GetString(_message));
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
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, _packetCount, _packetIndex, 0x00, 0x00, 0x01, 0x4D, 0x53, 0x47,
                    0x00, 0x00, 0x00, 0x00, 0x00
                });
                bodyWriter.Write(SendXML(_DateTime, compressMsg));
            }
            else if (_messageType.HasFlag(MessageType.Shake))
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
            }
            else
            {
                bodyWriter.BEWrite(user.QQ);
                bodyWriter.BEWrite(_toQQ);
                bodyWriter.Write(new byte[]
                {
                    0x00, 0x00, 0x00, 0x0D, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x01, 0x01
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
                    0x02, 0x34, 0x00, 0x00, 0x00, 0x00, _packetCount, _packetIndex, 0x00, 0x00, 0x01, 0x4D, 0x53, 0x47,
                    0x00, 0x00, 0x00, 0x00, 0x00
                });
                bodyWriter.BEWrite(_DateTime);
                bodyWriter.Write(Util.RandomKey(4));
                bodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                bodyWriter.Write(new byte[] {0x00, 0x06});
                bodyWriter.Write(new byte[] {0xE5, 0xAE, 0x8B, 0xE4, 0xBD, 0x93});
                bodyWriter.Write(new byte[] {0x00, 0x00});

                if (Encoding.UTF8.GetString(_message).Contains("[face") &&
                    Encoding.UTF8.GetString(_message).Contains(".gif]"))
                {
                    var MessageData = ConstructMessage(Encoding.UTF8.GetString(_message));
                    if (MessageData.Length != 0)
                    {
                        bodyWriter.Write(MessageData);
                    }
                }
                else
                {
                    //普通消息
                    ConstructMessage(bodyWriter, _message);
                }
            }
        }

        public static List<Send_0x00CD> SendLongMessage(QQUser User, string Message, MessageType messageType,
            long ToQQ)
        {
            var buffer = new BinaryWriter(new MemoryStream());
            var list = new List<byte[]>();
            foreach (var chr in Message)
            {
                var bytes = Encoding.UTF8.GetBytes(chr.ToString());
                if (buffer.BaseStream.Length + bytes.Length > 699)
                {
                    list.Add(buffer.BaseStream.ToBytesArray());
                    buffer = new BinaryWriter(new MemoryStream());
                }

                buffer.Write(bytes);
            }

            if (buffer.BaseStream.Position != 0)
            {
                list.Add(buffer.BaseStream.ToBytesArray());
            }

            byte index = 0;
            var ret = new List<Send_0x00CD>();
            foreach (var byteBuffer in list)
            {
                ret.Add(new Send_0x00CD(User, "", messageType, ToQQ)
                {
                    _packetCount = (byte) list.Count,
                    _packetIndex = index++,
                    _message = byteBuffer
                });
            }

            return ret;
        }
    }
}