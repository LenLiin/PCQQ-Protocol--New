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
        private readonly byte _packetIndex;

        public List<Send_0X0002> Following;
        private readonly byte[] _data;

        public Send_0X0002(QQUser user, Richtext message, long group)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Message0X0002;
            Message = message;
            _group = group;
            Following = new List<Send_0X0002>();
            var data = Util.WriteRichtext(message);
            var count = data.Count;
            var index = 1;
            _data = data[0];
            data.RemoveAt(0);
            foreach (var part in data)
            {
                Following.Add(new Send_0X0002(user, part, group, index++, count));
            }
        }

        private Send_0X0002(QQUser user, byte[] data, long group, int index, int count) : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Message0X0002;
            _data = data;
            _group = group;
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
            var group = ConvertQQGroupId(_group);
            switch (Message.Snippets[0].Type)
            {
                case MessageType.At:
                case MessageType.Normal:
                case MessageType.Emoji:
                {
                    BodyWriter.Write((byte) 0x2A);
                    BodyWriter.BeWrite(group);
                    BodyWriter.BeWrite((ushort) (_data.Length + 50)); // 字符串长度 + 56，但是_data里面包含了和长度有关的额外六个byte
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
                    BodyWriter.Write(_data);
                    break;
                }
                case MessageType.Xml:
                {
                    BodyWriter.Write((byte) 0x2A);
                    BodyWriter.BeWrite(group);
                    var compressMsg = GZipByteArray.CompressBytes(Message.Snippets[0].Content);
                    BodyWriter.BeWrite((ushort) (compressMsg.Length + 64));
                    BodyWriter.Write(new byte[]
                    {
                        0x00, 0x01, _packetCount, _packetIndex, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4D, 0x53,
                        0x47, 0x00,
                        0x00, 0x00, 0x00, 0x00
                    });
                    BodyWriter.BeWrite(dateTime);
                    BodyWriter.Write(SendXml(compressMsg));
                    break;
                }
                case MessageType.Picture:
                {
                    HttpUpLoadGroupImg(_group, User.Ukey, Message.Snippets[0].Content);
                    BodyWriter.Write((byte) 0x2A);
                    BodyWriter.BeWrite(group);
                    var guid = Encoding.UTF8.GetBytes(Util.GetMD5ToGuidHashFromFile(Message.Snippets[0].Content));
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
                    var messageData = Encoding.UTF8.GetBytes(Message.Snippets[0].Content);
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
                default:
                {
                    throw new NotImplementedException();
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
            var group = code.ToString();
            var left = Convert.ToInt64(group.Substring(0, group.Length - 6));
            string right = "", gid = "";
            if (left >= 1 && left <= 10)
            {
                right = group.Substring(group.Length - 6, 6);
                gid = left + 202 + right;
            }
            else if (left >= 11 && left <= 19)
            {
                right = group.Substring(group.Length - 6, 6);
                gid = left + 469 + right;
            }
            else if (left >= 20 && left <= 66)
            {
                left = Convert.ToInt64(left.ToString().Substring(0, 1));
                right = group.Substring(group.Length - 7, 7);
                gid = left + 208 + right;
            }
            else if (left >= 67 && left <= 156)
            {
                right = group.Substring(group.Length - 6, 6);
                gid = left + 1943 + right;
            }
            else if (left >= 157 && left <= 209)
            {
                left = Convert.ToInt64(left.ToString().Substring(0, 2));
                right = group.Substring(group.Length - 7, 7);
                gid = left + 199 + right;
            }
            else if (left >= 210 && left <= 309)
            {
                left = Convert.ToInt64(left.ToString().Substring(0, 2));
                right = group.Substring(group.Length - 7, 7);
                gid = left + 389 + right;
            }
            else if (left >= 310 && left <= 499)
            {
                left = Convert.ToInt64(left.ToString().Substring(0, 2));
                right = group.Substring(group.Length - 7, 7);
                gid = left + 349 + right;
            }
            else
            {
                return code;
            }

            return Convert.ToInt64(gid);
        }
    }
}