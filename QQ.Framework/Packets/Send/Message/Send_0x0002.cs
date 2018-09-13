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
        public Send_0x0002(QQUser User, byte[] Message) : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Message0x0002;
            _message = Message;
        }

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

        /// <summary>
        ///     上传图片
        /// </summary>
        /// <param name="GroupNum"></param>
        /// <param name="Ukey"></param>
        /// <param name="FileName"></param>
        public static void HttpUpLoadGroupImg(QQUser user, long GroupNum, string Ukey, string FileName)
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

        public static long GroupToGid(long groupid)
        {
            var group = groupid.ToString();
            var left = Convert.ToInt64(group.Substring(0, group.Length - 6));
            string right, gid;
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
                return groupid;
            }

            return Convert.ToInt64(gid);
        }

        public static List<Send_0x0002> SendLongMessage(QQUser user, Richtext message, long groupNum)
        {
            throw new NotImplementedException();
        }

        public static List<byte[]> ConstructMessage(QQUser user, Richtext message, long groupNum)
        {
            var dateTime = Util.GetTimeSeconds(DateTime.Now);
            var group = GroupToGid(groupNum);
            var ret = new List<byte[]>();
            var writer = new BinaryWriter(new MemoryStream());
            // FIXME: 使用正确的_packetCount和_packetIndex进行分段
            byte _packetCount = 1, _packetIndex = 0;
            foreach (var snippet in message.Snippets)
            {
                switch (snippet.Type)
                {
                    case MessageType.Xml:
                    {
                        writer.Write((byte) 0x2A);
                        writer.BEWrite(group);
                        var compressMsg = GZipByteArray.CompressBytes(snippet.Content);
                        writer.BEWrite((ushort) (compressMsg.Length + 64));
                        writer.Write(new byte[]
                        {
                            0x00, 0x01, _packetCount, _packetIndex, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4D, 0x53,
                            0x47, 0x00,
                            0x00, 0x00, 0x00, 0x00
                        });
                        writer.BEWrite(dateTime);
                        writer.Write(Util.RandomKey(4));
                        writer.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                        writer.Write(new byte[] {0x00, 0x0C});
                        writer.Write(new byte[]
                            {0xE5, 0xBE, 0xAE, 0xE8, 0xBD, 0xAF, 0xE9, 0x9B, 0x85, 0xE9, 0xBB, 0x91});
                        writer.Write(new byte[] {0x00, 0x00, 0x14});
                        writer.BEWrite((ushort) (compressMsg.Length + 11));
                        writer.Write((byte) 0x01);
                        writer.BEWrite((ushort) (compressMsg.Length + 1));
                        writer.Write((byte) 0x01);
                        writer.Write(compressMsg);
                        writer.Write(new byte[] {0x02, 0x00, 0x04, 0x00, 0x00, 0x00, 0x4D});
                        break;
                    }
                    case MessageType.Picture:
                    {
                        HttpUpLoadGroupImg(user, groupNum, user.Ukey, snippet.Content);
                        writer.Write((byte) 0x2A);
                        writer.BEWrite(group);
                        writer.Write(new byte[]
                        {
                            0x00, 0x01, _packetCount, _packetIndex, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4D, 0x53,
                            0x47, 0x00,
                            0x00, 0x00, 0x00, 0x00
                        });
                        var guid = Encoding.UTF8.GetBytes(Util.GetMD5ToGuidHashFromFile(snippet.Content));
                        writer.BEWrite(dateTime);
                        writer.Write(Util.RandomKey(4));
                        writer.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                        writer.Write(new byte[]
                            {0xE5, 0xBE, 0xAE, 0xE8, 0xBD, 0xAF, 0xE9, 0x9B, 0x85, 0xE9, 0xBB, 0x91});
                        writer.Write(new byte[] {0x00, 0x00, 0x03, 0x00, 0xCB, 0x02});
                        writer.Write(new byte[] {0x00, 0x2A});
                        writer.Write(guid);
                        writer.Write(new byte[] {0x04, 0x00, 0x04});
                        writer.Write(new byte[]
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
                        writer.Write(new byte[]
                        {
                            0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x35, 0x30, 0x20, 0x20, 0x20, 0x20, 0x20,
                            0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20
                        });
                        writer.Write(guid);
                        writer.Write(0x41);
                        break;
                    }
                    case MessageType.AddGroup:
                    {
                        writer.Write(new byte[] {0x08});
                        writer.BEWrite(group);
                        writer.Write(new byte[] {0x01});
                        writer.BEWrite((ushort) user.AddFriend_0020Value.Length);
                        writer.Write(user.AddFriend_0020Value);
                        writer.Write(new byte[] {0x00, 0x00, 0x00});
                        //备注信息
                        var messageData = Encoding.UTF8.GetBytes(snippet.Content);
                        writer.BEWrite((ushort) messageData.Length);
                        writer.Write(messageData);
                        writer.Write(new byte[] {0x01, 0x00, 0x01, 0x00, 0x04, 0x00, 0x01, 0x00, 0x09});
                        break;
                    }
                    case MessageType.GetGroupImformation:
                    {
                        writer.Write(new byte[] {0x72});
                        writer.BEWrite(group);
                        break;
                    }
                    case MessageType.ExitGroup:
                    {
                        writer.Write(new byte[] {0x09});
                        writer.BEWrite(group);
                        break;
                    }
                    case MessageType.Normal:
                    {
                        writer.Write((byte) 0x2A);
                        writer.BEWrite(group);
                        var messageData = Encoding.UTF8.GetBytes(snippet.Content);
                        writer.BEWrite((ushort) (messageData.Length + 56));
                        writer.Write(new byte[]
                        {
                            0x00, 0x01, _packetCount, _packetIndex, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4D, 0x53,
                            0x47, 0x00,
                            0x00, 0x00, 0x00, 0x00
                        });
                        writer.BEWrite(dateTime);
                        writer.Write(Util.RandomKey(4));
                        writer.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                        writer.Write(new byte[] {0x00, 0x0C});
                        writer.Write(new byte[]
                            {0xE5, 0xBE, 0xAE, 0xE8, 0xBD, 0xAF, 0xE9, 0x9B, 0x85, 0xE9, 0xBB, 0x91});
                        writer.Write(new byte[] {0x00, 0x00});
                        writer.Write(new byte[] {0x01});
                        writer.BEWrite((ushort) (messageData.Length + 3));
                        writer.Write(new byte[] {0x01});
                        writer.BEWrite((ushort) messageData.Length);
                        writer.Write(messageData);
                        break;
                    }
                    case MessageType.Emoji:
                    {
                        writer.Write((byte) 0x2A);
                        writer.BEWrite(group);
                        var messageData = Encoding.UTF8.GetByteCount(snippet.Content);
                        writer.BEWrite((ushort) (messageData + 56));
                        writer.Write(new byte[]
                        {
                            0x00, 0x01, _packetCount, _packetIndex, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4D, 0x53,
                            0x47, 0x00,
                            0x00, 0x00, 0x00, 0x00
                        });
                        writer.BEWrite(dateTime);
                        writer.Write(Util.RandomKey(4));
                        writer.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                        writer.Write(new byte[] {0x00, 0x0C});
                        writer.Write(new byte[]
                            {0xE5, 0xBE, 0xAE, 0xE8, 0xBD, 0xAF, 0xE9, 0x9B, 0x85, 0xE9, 0xBB, 0x91});
                        writer.Write(new byte[] {0x00, 0x00});
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