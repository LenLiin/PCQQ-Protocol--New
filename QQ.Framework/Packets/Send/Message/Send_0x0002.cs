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
    public class Send_0X0002 : SendPacket
    {
        private readonly long _group;

        private readonly byte _packetCount = 1;
        private byte _packetIndex;

        public Send_0X0002(QQUser user, Richtext message, MessageType messageType, long @group)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Message0X0002;
            Message = message;
            MessageType = messageType;
            _group = @group;
        }

        /// <summary>
        ///     消息类型
        /// </summary>
        public MessageType MessageType { get; set; }

        private Richtext Message { get; }

        protected override void PutHeader()
        {
            base.PutHeader();
            Writer.Write(User.QQPacketFixver);
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            var dateTime = Util.GetTimeSeconds(DateTime.Now);
            var group = ConvertQQGroupId(_group);
            foreach (var snippet in Message.Snippets)
            {
                switch (snippet.Type)
                {
                    case MessageType.Xml:
                    {
                        BodyWriter.Write((byte) 0x2A);
                        BodyWriter.BeWrite(group);
                        var compressMsg = GZipByteArray.CompressBytes(snippet.Content);
                        BodyWriter.BeWrite((ushort) (compressMsg.Length + 64));
                        BodyWriter.Write(new byte[]
                        {
                            0x00, 0x01, _packetCount, _packetIndex, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4D, 0x53,
                            0x47, 0x00,
                            0x00, 0x00, 0x00, 0x00
                        });
                        BodyWriter.Write(SendXml(dateTime, compressMsg));
                        break;
                    }
                    case MessageType.Picture:
                    {
                        HttpUpLoadGroupImg(_group, User.UKey, snippet.Content);
                        BodyWriter.Write((byte) 0x2A);
                        BodyWriter.BeWrite(group);
                        var guid = Encoding.UTF8.GetBytes(Util.GetMD5ToGuidHashFromFile(snippet.Content));
                        BodyWriter.Write(new byte[]
                        {
                            0x00, 0x01, _packetCount, _packetIndex, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4D, 0x53,
                            0x47, 0x00,
                            0x00, 0x00, 0x00, 0x00
                        });
                        BodyWriter.BeWrite(dateTime);
                        BodyWriter.Write(Util.RandomKey(4));
                        BodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                        BodyWriter.Write(new byte[]
                            {0xE5, 0xBE, 0xAE, 0xE8, 0xBD, 0xAF, 0xE9, 0x9B, 0x85, 0xE9, 0xBB, 0x91});
                        BodyWriter.Write(new byte[] {0x00, 0x00, 0x03, 0x00, 0xCB, 0x02});
                        BodyWriter.Write(new byte[] {0x00, 0x2A});
                        BodyWriter.Write(guid);
                        BodyWriter.Write(new byte[] {0x04, 0x00, 0x04});
                        BodyWriter.Write(new byte[]
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
                        BodyWriter.Write(new byte[]
                        {
                            0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x35, 0x30, 0x20, 0x20, 0x20, 0x20, 0x20,
                            0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20
                        });
                        BodyWriter.Write(guid);
                        BodyWriter.Write(0x41);
                        break;
                    }
                    case MessageType.AddGroup:
                    {
                        BodyWriter.Write(new byte[] {0x08});
                        BodyWriter.BeWrite(group);
                        BodyWriter.Write(new byte[] {0x01});
                        BodyWriter.BeWrite((ushort) User.AddFriend0020Value.Length);
                        BodyWriter.Write(User.AddFriend0020Value);
                        BodyWriter.Write(new byte[] {0x00, 0x00, 0x00});
                        //备注信息
                        var messageData = Encoding.UTF8.GetBytes(snippet.Content);
                        Writer.BeWrite((ushort) messageData.Length);
                        Writer.Write(messageData);

                        BodyWriter.Write(new byte[] {0x01, 0x00, 0x01, 0x00, 0x04, 0x00, 0x01, 0x00, 0x09});
                        break;
                    }
                    case MessageType.GetGroupImformation:
                    {
                        BodyWriter.Write(new byte[] {0x72});
                        BodyWriter.BeWrite(group);
                        break;
                    }
                    case MessageType.ExitGroup:
                    {
                        BodyWriter.Write(new byte[] {0x09});
                        BodyWriter.BeWrite(group);
                        break;
                    }
                    case MessageType.Normal:
                    {
                        BodyWriter.Write((byte) 0x2A);
                        BodyWriter.BeWrite(group);
                        var messageData = Encoding.UTF8.GetBytes(snippet.Content);
                        BodyWriter.BeWrite((ushort) (messageData.Length + 56));
                        BodyWriter.Write(new byte[]
                        {
                            0x00, 0x01, _packetCount, _packetIndex, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4D, 0x53,
                            0x47, 0x00,
                            0x00, 0x00, 0x00, 0x00
                        });
                        BodyWriter.BeWrite(dateTime);
                        BodyWriter.Write(Util.RandomKey(4));
                        BodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                        BodyWriter.Write(new byte[] {0x00, 0x0C});
                        BodyWriter.Write(new byte[]
                            {0xE5, 0xBE, 0xAE, 0xE8, 0xBD, 0xAF, 0xE9, 0x9B, 0x85, 0xE9, 0xBB, 0x91});
                        BodyWriter.Write(new byte[] {0x00, 0x00});

                        ConstructMessage(BodyWriter, messageData);
                        break;
                    }
                    case MessageType.Emoji:
                    {
                        BodyWriter.Write((byte) 0x2A);
                        BodyWriter.BeWrite(group);
                        BodyWriter.BeWrite((ushort) (Encoding.UTF8.GetByteCount(snippet.Content) + 56));
                        BodyWriter.Write(new byte[]
                        {
                            0x00, 0x01, _packetCount, _packetIndex, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4D, 0x53,
                            0x47, 0x00,
                            0x00, 0x00, 0x00, 0x00
                        });
                        BodyWriter.BeWrite(dateTime);
                        BodyWriter.Write(Util.RandomKey(4));
                        BodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                        BodyWriter.Write(new byte[] {0x00, 0x0C});
                        BodyWriter.Write(new byte[]
                            {0xE5, 0xBE, 0xAE, 0xE8, 0xBD, 0xAF, 0xE9, 0x9B, 0x85, 0xE9, 0xBB, 0x91});
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

        /// <summary>
        ///     上传图片
        /// </summary>
        /// <param name="groupNum"></param>
        /// <param name="ukey"></param>
        /// <param name="fileName"></param>
        public void HttpUpLoadGroupImg(long groupNum, string ukey, string fileName)
        {
            using (var webclient = new WebClient())
            {
                var file = new FileStream(fileName, FileMode.Open);
                var apiUrl =
                    $"http://htdata2.qq.com/cgi-bin/httpconn?htcmd=0x6ff0071&ver=5515&term=pc&ukey={ukey}&filesize={file.Length}&range=0&uin{User.QQ}&&groupcode={groupNum}";
                webclient.Headers["User-Agent"] = "QQClient";
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                var result = webclient.UploadData(apiUrl,
                    file.ToBytesArray());

                Console.Write(Encoding.UTF8.GetString(result));
            }
        }

        public long ConvertQQGroupId(long code)
        {
            var text = code.ToString();
            var text2 = text.Substring(0, text.Length - 6);
            var arg = text.Substring(text.Length - 6, 6);
            var num = 0u;
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

        public static List<Send_0X0002> SendLongMessage(QQUser user, Richtext message, long @group)
        {
            throw new NotImplementedException();
        }
    }
}