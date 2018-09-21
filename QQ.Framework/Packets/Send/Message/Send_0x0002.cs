using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Message
{
    /// <summary>
    ///     发送消息
    /// </summary>
    public class Send_0x0002 : SendPacket
    {
        private readonly long _group;

        private readonly byte _packetCount = 1;
        private byte _packetIndex;

        public Send_0x0002(QQUser User, Richtext Message, MessageType messageType, long Group)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = User.TXProtocol.SessionKey;
            Command = QQCommand.Message0x0002;
            _message = Message;
            _messageType = messageType;
            _group = Group;
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
            var group = ConvertQQGroupId(_group);
            foreach (var snippet in _message.Snippets)
            {
                switch (snippet.Type)
                {
                    case MessageType.Xml:
                    {
                        bodyWriter.Write((byte) 0x2A);
                        bodyWriter.BEWrite(group);
                        var compressMsg = GZipByteArray.CompressBytes(snippet.Content);
                        bodyWriter.BEWrite((ushort) (compressMsg.Length + 64));
                        bodyWriter.Write(new byte[]
                        {
                            0x00, 0x01, _packetCount, _packetIndex, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4D, 0x53,
                            0x47, 0x00,
                            0x00, 0x00, 0x00, 0x00
                        });
                        bodyWriter.Write(SendXML(_DateTime, compressMsg));
                        break;
                    }
                    case MessageType.Picture:
                    {
                        HttpUpLoadGroupImg(_group, user.Ukey, snippet.Content);
                        bodyWriter.Write((byte) 0x2A);
                        bodyWriter.BEWrite(group);
                        var Guid = Encoding.UTF8.GetBytes(Util.GetMD5ToGuidHashFromFile(snippet.Content));
                        bodyWriter.Write(new byte[]
                        {
                            0x00, 0x01, _packetCount, _packetIndex, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4D, 0x53,
                            0x47, 0x00,
                            0x00, 0x00, 0x00, 0x00
                        });
                        bodyWriter.BEWrite(_DateTime);
                        bodyWriter.Write(Util.RandomKey(4));
                        bodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                        bodyWriter.Write(new byte[]
                            {0xE5, 0xBE, 0xAE, 0xE8, 0xBD, 0xAF, 0xE9, 0x9B, 0x85, 0xE9, 0xBB, 0x91});
                        bodyWriter.Write(new byte[] {0x00, 0x00, 0x03, 0x00, 0xCB, 0x02});
                        bodyWriter.Write(new byte[] {0x00, 0x2A});
                        bodyWriter.Write(Guid);
                        bodyWriter.Write(new byte[] {0x04, 0x00, 0x04});
                        bodyWriter.Write(new byte[]
                        {
                            0x9B, 0x53, 0xB0, 0x08, 0x05, 0x00, 0x04, 0xD9, 0x8A, 0x5A, 0x70, 0x06, 0x00,
                            0x04, 0x00, 0x00, 0x00, 0x50, 0x07, 0x00, 0x01, 0x43, 0x08, 0x00, 0x00, 0x09, 0x00, 0x01,
                            0x01,
                            0x0B,
                            0x00, 0x00, 0x14, 0x00, 0x04, 0x11, 0x00, 0x00, 0x00, 0x15, 0x00, 0x04, 0x00, 0x00, 0x02,
                            0xBC,
                            0x16,
                            0x00, 0x04, 0x00, 0x00, 0x02, 0xBC, 0x18, 0x00, 0x04, 0x00, 0x00, 0x7D, 0x5E, 0xFF, 0x00,
                            0x5C,
                            0x15,
                            0x36, 0x20, 0x39, 0x32, 0x6B, 0x41, 0x31, 0x43, 0x39, 0x62, 0x35, 0x33, 0x62, 0x30, 0x30,
                            0x38,
                            0x64,
                            0x39, 0x38, 0x61, 0x35, 0x61, 0x37, 0x30
                        });
                        bodyWriter.Write(new byte[]
                        {
                            0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x35, 0x30, 0x20, 0x20, 0x20, 0x20, 0x20,
                            0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20
                        });
                        bodyWriter.Write(Guid);
                        bodyWriter.Write(0x41);
                        break;
                    }
                    case MessageType.AddGroup:
                    {
                        bodyWriter.Write(new byte[] {0x08});
                        bodyWriter.BEWrite(group);
                        bodyWriter.Write(new byte[] {0x01});
                        bodyWriter.BEWrite((ushort) user.AddFriend_0020Value.Length);
                        bodyWriter.Write(user.AddFriend_0020Value);
                        bodyWriter.Write(new byte[] {0x00, 0x00, 0x00});
                        //备注信息
                        var _messageData = Encoding.UTF8.GetBytes(snippet.Content);
                        writer.BEWrite((ushort) _messageData.Length);
                        writer.Write(_messageData);

                        bodyWriter.Write(new byte[] {0x01, 0x00, 0x01, 0x00, 0x04, 0x00, 0x01, 0x00, 0x09});
                        break;
                    }
                    case MessageType.GetGroupImformation:
                    {
                        bodyWriter.Write(new byte[] {0x72});
                        bodyWriter.BEWrite(group);
                        break;
                    }
                    case MessageType.ExitGroup:
                    {
                        bodyWriter.Write(new byte[] {0x09});
                        bodyWriter.BEWrite(group);
                        break;
                    }
                    case MessageType.Normal:
                    {
                        bodyWriter.Write((byte) 0x2A);
                        bodyWriter.BEWrite(group);
                        var _messageData = Encoding.UTF8.GetBytes(snippet.Content);
                        bodyWriter.BEWrite((ushort) (_messageData.Length + 56));
                        bodyWriter.Write(new byte[]
                        {
                            0x00, 0x01, _packetCount, _packetIndex, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4D, 0x53,
                            0x47, 0x00,
                            0x00, 0x00, 0x00, 0x00
                        });
                        bodyWriter.BEWrite(_DateTime);
                        bodyWriter.Write(Util.RandomKey(4));
                        bodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                        bodyWriter.Write(new byte[] {0x00, 0x0C});
                        bodyWriter.Write(new byte[]
                            {0xE5, 0xBE, 0xAE, 0xE8, 0xBD, 0xAF, 0xE9, 0x9B, 0x85, 0xE9, 0xBB, 0x91});
                        bodyWriter.Write(new byte[] {0x00, 0x00});

                        ConstructMessage(bodyWriter, _messageData);
                        break;
                    }
                    case MessageType.Emoji:
                    {
                        bodyWriter.Write((byte) 0x2A);
                        bodyWriter.BEWrite(group);
                        var _messageData = Encoding.UTF8.GetBytes(snippet.Content);
                        bodyWriter.BEWrite((ushort) (_messageData.Length + 56));
                        bodyWriter.Write(new byte[]
                        {
                            0x00, 0x01, _packetCount, _packetIndex, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4D, 0x53,
                            0x47, 0x00,
                            0x00, 0x00, 0x00, 0x00
                        });
                        bodyWriter.BEWrite(_DateTime);
                        bodyWriter.Write(Util.RandomKey(4));
                        bodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                        bodyWriter.Write(new byte[] {0x00, 0x0C});
                        bodyWriter.Write(new byte[]
                            {0xE5, 0xBE, 0xAE, 0xE8, 0xBD, 0xAF, 0xE9, 0x9B, 0x85, 0xE9, 0xBB, 0x91});
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

        /// <summary>
        ///     上传图片
        /// </summary>
        /// <param name="GroupNum"></param>
        /// <param name="Ukey"></param>
        /// <param name="FileName"></param>
        public void HttpUpLoadGroupImg(long GroupNum, string Ukey, string FileName)
        {
            using (var webclient = new WebClient())
            {
                var file = new FileStream(FileName, FileMode.Open);
                var ApiUrl =
                    $"http://htdata2.qq.com/cgi-bin/httpconn?htcmd=0x6ff0071&ver=5515&term=pc&ukey={Ukey}&filesize={file.Length}&range=0&uin{user.QQ}&&groupcode={GroupNum}";
                webclient.Headers["User-Agent"] = "QQClient";
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                var result = webclient.UploadData(ApiUrl,
                    file.ToBytesArray());

                Console.Write(Encoding.UTF8.GetString(result));
            }
        }

        public long ConvertQQGroupId(long Code)
        {
            string text = Code.ToString();
            string text2 = text.Substring(0, text.Length - 6);
            string arg = text.Substring(text.Length - 6, 6);
            uint num = 0u;
            if (text2.Length <= 0)
            {
                num += 202;
            }
            else if (num < 11)
            {
                num += 202;
            }
            else if (num < 20)
            {
                num += 469;
            }
            else if (num < 67)
            {
                num += 2080;
            }
            else if (num < 157)
            {
                num += 1943;
            }
            else if (num < 210)
            {
                num += 1990;
            }
            else if (num < 310)
            {
                num += 3890;
            }
            else if (num < 336)
            {
                num += 3490;
            }
            return long.Parse($"{num}{arg}");
        }

        public static List<Send_0x0002> SendLongMessage(QQUser User, Richtext Message, long Group)
        {
            throw new NotImplementedException();
        }
    }
}