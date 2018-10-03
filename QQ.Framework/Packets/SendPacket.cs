using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets
{
    public abstract class SendPacket : Packet
    {
        /// <summary>
        ///     包起始序列号
        /// </summary>
        protected static char _seq = (char) 0x3635; // (char)Util.Random.Next();

        public MemoryStream BodyStream;
        public BinaryWriter BodyWriter;

        public BinaryWriter Writer = new BinaryWriter(new MemoryStream());

        public SendPacket()
        {
        }

        /// <summary>
        ///     构造一个指定参数的包
        /// </summary>
        public SendPacket(QQUser user)
            : base(user)
        {
            Version = QQGlobal.QQClientVersion;
        }

        /// <summary>
        ///     加密包体
        /// </summary>
        /// <param name="buf">未加密的字节数组.</param>
        /// <param name="offset">包体开始的偏移.</param>
        /// <param name="length">包体长度.</param>
        /// <returns>加密的包体</returns>
        public byte[] EncryptBody(byte[] buf, int offset, int length)
        {
            return QQTea.Encrypt(buf, offset, length, SecretKey);
        }

        /// <summary>
        ///     将包头部写入流。
        /// </summary>
        protected virtual void PutHeader()
        {
            Writer.Write(QQGlobal.QQHeaderBasicFamily);
            Writer.Write(User.TXProtocol.CMainVer);
            Writer.Write(User.TXProtocol.CSubVer);
            Writer.BeWrite((ushort) Command);
            Writer.BeWrite(Sequence);
            Writer.BeWrite(User.QQ);
        }

        /// <summary>
        ///     包头描述部分
        /// </summary>
        protected void SendPACKET_FIX()
        {
            Writer.Write(User.TXProtocol.XxooA);
            Writer.Write(User.TXProtocol.DwClientType);
            Writer.Write(User.TXProtocol.DwPubNo);
            Writer.Write(User.TXProtocol.XxooD);
        }

        protected static char GetNextSeq()
        {
            _seq++;
            // 为了兼容iQQ
            // iQQ把序列号的高位都为0，如果为1，它可能会拒绝，wqfox称是因为TX是这样做的
            _seq &= (char) 0x7FFF;
            if (_seq == 0)
            {
                _seq++;
            }

            return _seq;
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected abstract void PutBody();

        /// <summary>
        ///     将包尾部转化为字节流, 写入指定的ByteBuffer对象.
        /// </summary>
        protected virtual void PutTail()
        {
            Writer.Write(QQGlobal.QQHeader03Family);
        }

        /// <summary>
        ///     将整个包转化为字节流, 并返回其值。
        ///     可直接使用QQClient.Send(new Send_0x__().WriteData())。
        /// </summary>
        public byte[] WriteData()
        {
            //保存当前pos
            var pos = (int) Writer.BaseStream.Position;
            //填充头部
            PutHeader();
            //填充包体
            BodyStream = new MemoryStream();
            BodyWriter = new BinaryWriter(BodyStream);
            PutBody();
            //需要加密的包体
            BodyDecrypted = BodyStream.ToArray();
            var enc = EncryptBody(BodyDecrypted, 0, BodyDecrypted.Length);
            // 加密内容写入最终buf
            Writer.Write(enc);
            // 填充尾部
            PutTail();
            // 回填
            PostFill(pos);
            return Writer.BaseStream.ToBytesArray();
        }

        /// <summary>
        ///     回填，有些字段必须填完整个包才能确定其内容，比如长度字段，那么这个方法将在
        ///     尾部填充之后调用
        /// </summary>
        /// <param name="startPos">The start pos.</param>
        public void PostFill(int startPos)
        {
            // 如果是tcp包，到包的开头处填上包长度，然后回到目前的pos
            if (!User.IsUdp)
            {
                var len = (int) (Writer.BaseStream.Length - startPos);
                var currentPos = Writer.BaseStream.Position;
                Writer.BaseStream.Position = startPos;
                Writer.BeWrite((ushort) len);
                Writer.BaseStream.Position = currentPos;
            }
        }

        /// <summary>
        ///     得到包体的字节数组
        /// </summary>
        /// <param name="length">包总长度</param>
        /// <returns>包体字节数组</returns>
        protected byte[] GetBodyBytes(int length)
        {
            var buf = new byte[QQGlobal.QQPacketMaxSize];
            // 得到包体长度
            var bodyLen = length - QQGlobal.QQLengthBasicFamilyOutHeader - QQGlobal.QQLengthBasicFamilyTail;
            if (!User.IsUdp)
            {
                bodyLen -= 2;
            }

            // 得到加密的包体内容
            // 没看懂，这个buf根本没被赋值，怎么读取的
            // byte[] body = buf.ReadBytes(bodyLen);
            return null;
        }

        /// <summary>
        ///     带表情消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [Obsolete("请使用BinaryWriter.Write(Richtext)方法。")]
        public static byte[] ConstructMessage(string message)
        {
            var bw = new BinaryWriter(new MemoryStream());
            var r = new Regex(@"([^\[]+)*(\[face\d+\.gif\])([^\[]+)*");
            if (r.IsMatch(message))
            {
                var faces = r.Matches(message);
                for (var i = 0; i < faces.Count; i++)
                {
                    var face = faces[i];
                    for (var j = 1; j < face.Groups.Count; j++)
                    {
                        var group = face.Groups[j].Value;
                        if (group.Contains("[face") && group.Contains(".gif]"))
                        {
                            var faceIndex =
                                Convert.ToByte(group.Substring(5, group.Length - group.LastIndexOf(".") - 4));
                            if (faceIndex > 199)
                            {
                                faceIndex = 0;
                            }

                            //表情
                            bw.Write(new byte[] {0x02, 0x00, 0x14, 0x01, 0x00, 0x01});
                            bw.Write(faceIndex);
                            bw.Write(new byte[] {0xFF, 0x00, 0x02, 0x14});
                            bw.Write((byte) (faceIndex + 65));
                            bw.Write(new byte[] {0x0B, 0x00, 0x08, 0x00, 0x01, 0x00, 0x04, 0x52, 0xCC, 0x85, 0x50});
                        }
                        else if (!string.IsNullOrEmpty(group))
                        {
                            var groupMsg = Encoding.UTF8.GetBytes(group);
                            //普通消息
                            ConstructMessage(bw, groupMsg);
                        }
                    }
                }
            }

            return bw.BaseStream.ToBytesArray();
        }

        /// <summary>
        ///     普通消息
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="groupMsg"></param>
        [Obsolete("请使用BinaryWriter.Write(Richtext)方法。")]
        public static void ConstructMessage(BinaryWriter writer, byte[] groupMsg)
        {
            writer.Write(new byte[] {0x01});
            writer.BeWrite((ushort) (groupMsg.Length + 3));
            writer.Write(new byte[] {0x01});
            writer.BeWrite((ushort) groupMsg.Length);
            writer.Write(groupMsg);
        }

        public static void SendAudio(uint qq, string file)
        {
        }

        public static void SendOfflineFile(uint qq, string file)
        {
        }

        /// <summary>
        ///     XML消息组装
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <param name="compressMsg">压缩消息数组</param>
        [Obsolete("请使用BinaryWriter.Write(Richtext)方法。")]
        public static byte[] SendXml(byte[] compressMsg)
        {
            var bw = new BinaryWriter(new MemoryStream());
            bw.Write(Util.RandomKey(4));
            bw.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00});
            bw.Write(new byte[] {0x00, 0x0C});
            bw.Write(new byte[] {0xE5, 0xBE, 0xAE, 0xE8, 0xBD, 0xAF, 0xE9, 0x9B, 0x85, 0xE9, 0xBB, 0x91});
            bw.Write(new byte[] {0x00, 0x00, 0x14});
            bw.BeWrite((ushort) (compressMsg.Length + 11));
            bw.Write((byte) 0x01);
            bw.BeWrite((ushort) (compressMsg.Length + 1));
            bw.Write((byte) 0x01);
            bw.Write(compressMsg);
            bw.Write(new byte[] {0x02, 0x00, 0x04, 0x00, 0x00, 0x00, 0x4D});
            return bw.BaseStream.ToBytesArray();
        }

        public static byte[] SendJson(string message)
        {
            throw new Exception();
        }
    }
}