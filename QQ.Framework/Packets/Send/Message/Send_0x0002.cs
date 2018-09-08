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

        private byte _packetCount = 1;
        private byte _packetIndex;

        public Send_0x0002(QQUser User, string Message, MessageType messageType, long Group)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Message0x0002;
            _message = Encoding.UTF8.GetBytes(Message);
            _messageType = messageType;
            _group = Group;
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
        protected override void PutBody()
        {
            var _DateTime = Util.GetTimeSeconds(DateTime.Now);
            var group = GroupToGid(_group);
            if (_messageType.HasFlag(MessageType.Xml))
            {
                bodyWriter.Write((byte) 0x2A);
                bodyWriter.BEWrite(group);
                var compressMsg = GZipByteArray.CompressBytes(Encoding.UTF8.GetString(_message));
                bodyWriter.BEWrite((ushort) (compressMsg.Length + 64));
                bodyWriter.Write(new byte[]
                {
                    0x00, 0x01, _packetCount, _packetIndex, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4D, 0x53, 0x47, 0x00,
                    0x00, 0x00, 0x00, 0x00
                });
                bodyWriter.Write(SendXML(_DateTime, compressMsg));
            }
            else if (_messageType.HasFlag(MessageType.Picture))
            {
                HttpUpLoadGroupImg(_group, user.Ukey, Encoding.UTF8.GetString(_message));
                bodyWriter.Write((byte) 0x2A);
                bodyWriter.BEWrite(group);
                var Guid = Encoding.UTF8.GetBytes(Util.GetMD5ToGuidHashFromFile(Encoding.UTF8.GetString(_message)));
                bodyWriter.Write(new byte[]
                {
                    0x00, 0x01, _packetCount, _packetIndex, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4D, 0x53, 0x47, 0x00,
                    0x00, 0x00, 0x00, 0x00
                });
                bodyWriter.BEWrite(_DateTime);
                bodyWriter.Write(Util.RandomKey(4));
                bodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                bodyWriter.Write(new byte[] {0xE5, 0xBE, 0xAE, 0xE8, 0xBD, 0xAF, 0xE9, 0x9B, 0x85, 0xE9, 0xBB, 0x91});
                bodyWriter.Write(new byte[] {0x00, 0x00, 0x03, 0x00, 0xCB, 0x02});
                bodyWriter.Write(new byte[] {0x00, 0x2A});
                bodyWriter.Write(Guid);
                bodyWriter.Write(new byte[] {0x04, 0x00, 0x04});
                bodyWriter.Write(new byte[]
                {
                    0x9B, 0x53, 0xB0, 0x08, 0x05, 0x00, 0x04, 0xD9, 0x8A, 0x5A, 0x70, 0x06, 0x00,
                    0x04, 0x00, 0x00, 0x00, 0x50, 0x07, 0x00, 0x01, 0x43, 0x08, 0x00, 0x00, 0x09, 0x00, 0x01, 0x01,
                    0x0B,
                    0x00, 0x00, 0x14, 0x00, 0x04, 0x11, 0x00, 0x00, 0x00, 0x15, 0x00, 0x04, 0x00, 0x00, 0x02, 0xBC,
                    0x16,
                    0x00, 0x04, 0x00, 0x00, 0x02, 0xBC, 0x18, 0x00, 0x04, 0x00, 0x00, 0x7D, 0x5E, 0xFF, 0x00, 0x5C,
                    0x15,
                    0x36, 0x20, 0x39, 0x32, 0x6B, 0x41, 0x31, 0x43, 0x39, 0x62, 0x35, 0x33, 0x62, 0x30, 0x30, 0x38,
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
            }
            else if (_messageType.HasFlag(MessageType.AddGroup))
            {
                bodyWriter.Write(new byte[] {0x08});
                bodyWriter.BEWrite(group);
                bodyWriter.Write(new byte[] {0x01});
                bodyWriter.BEWrite((ushort) user.AddFriend_0020Value.Length);
                bodyWriter.Write(user.AddFriend_0020Value);
                bodyWriter.Write(new byte[] {0x00, 0x00, 0x00});
                //备注信息
                writer.BEWrite((ushort) _message.Length);
                writer.Write(_message);

                bodyWriter.Write(new byte[] {0x01, 0x00, 0x01, 0x00, 0x04, 0x00, 0x01, 0x00, 0x09});
            }
            else if (_messageType.HasFlag(MessageType.GetGroupImformation)) //获取群组信息
            {
                bodyWriter.Write(new byte[] {0x72});
                bodyWriter.BEWrite(group);
            }
            else if (_messageType.HasFlag(MessageType.ExitGroup))
            {
                bodyWriter.Write(new byte[] {0x09});
                bodyWriter.BEWrite(group);
            }
            else // Send_0x0002是群消息，不需要进行判断
            {
                bodyWriter.Write((byte) 0x2A);
                bodyWriter.BEWrite(group);
                bodyWriter.BEWrite((ushort) (_message.Length + 56));
                bodyWriter.Write(new byte[]
                {
                    0x00, 0x01, _packetCount, _packetIndex, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4D, 0x53, 0x47, 0x00,
                    0x00, 0x00, 0x00, 0x00
                });
                bodyWriter.BEWrite(_DateTime);
                bodyWriter.Write(Util.RandomKey(4));
                bodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
                bodyWriter.Write(new byte[] {0x00, 0x0C});
                bodyWriter.Write(new byte[] {0xE5, 0xBE, 0xAE, 0xE8, 0xBD, 0xAF, 0xE9, 0x9B, 0x85, 0xE9, 0xBB, 0x91});
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
                var ApiUrl = $"http://htdata2.qq.com/cgi-bin/httpconn?htcmd=0x6ff0071&ver=5515&term=pc&ukey={Ukey}&filesize={file.Length}&range=0&uin{user.QQ}&&groupcode={GroupNum}";
                webclient.Headers["User-Agent"] = "QQClient";
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                var result = webclient.UploadData(ApiUrl,
                    file.ToBytesArray());

                Console.Write(Encoding.UTF8.GetString(result));
            }
        }

        public long GroupToGid(long groupid)
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

        public static List<Send_0x0002> SendLongMessage(QQUser User, string Message, MessageType messageType,
            long Group)
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
            var ret = new List<Send_0x0002>();
            foreach (var byteBuffer in list)
            {
                ret.Add(new Send_0x0002(User, "", messageType, Group)
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