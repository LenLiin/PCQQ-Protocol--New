using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QQ.Framework.Packets
{
    public abstract class SendPacket : Packet
    {
        public SendPacket() : base()
        {

        }
        /// <summary>
        /// 构造一个指定参数的包
        /// </summary>
        public SendPacket(QQUser User)
            : base(User)
        {
            Version = QQGlobal.QQ_CLIENT_VERSION;
        }

        /// <summary>
        /// 加密包体
        /// </summary>
        /// <param name="buf">未加密的字节数组.</param>
        /// <param name="offset">包体开始的偏移.</param>
        /// <param name="length">包体长度.</param>
        /// <returns>加密的包体</returns>
        public byte[] EncryptBody(byte[] buf, int offset, int length)
        {
            return QQTea.Encrypt(buf, offset, length, _secretKey);
        }
        /// <summary>
        /// 包起始序列号
        /// </summary>
        protected static char seq = (char)Util.Random.Next();
        /// <summary>
        /// 将包头部转化为字节流, 写入指定的ByteBuffer对象.
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected virtual void PutHeader(ByteBuffer buf)
        {
            buf.Put(QQGlobal.QQ_HEADER_BASIC_FAMILY);
            buf.PutChar(Version);
            buf.PutUShort((ushort)Command);
            buf.PutChar(Sequence);
            buf.PutLong(user.QQ);
        }
        protected static char GetNextSeq()
        {
            seq++;
            // 为了兼容iQQ
            // iQQ把序列号的高位都为0，如果为1，它可能会拒绝，wqfox称是因为TX是这样做的
            seq &= (char)0x7FFF;
            if (seq == 0)
            {
                seq++;
            }
            return seq;
        }
        /// <summary>
        /// 初始化包体
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected abstract void PutBody(ByteBuffer buf);

        /// <summary>
        /// 将包尾部转化为字节流, 写入指定的ByteBuffer对象.
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected virtual void PutTail(ByteBuffer buf)
        {
            buf.Put(QQGlobal.QQ_HEADER_03_FAMILY);
        }
        /// <summary>
        ///  将整个包转化为字节流, 并写入指定的ByteBuffer对象.
        ///  一般而言, 前后分别需要写入包头部和包尾部.
        /// </summary>
        /// <param name="buf">The buf.</param>
        public void Fill(ByteBuffer buf)
        {
            //保存当前pos
            int pos = buf.Position;
            //填充头部
            PutHeader(buf);
            //填充包体
            bodyBuf.Initialize();
            PutBody(bodyBuf);
            //需要加密的包体
            bodyDecrypted = bodyBuf.ToByteArray();
            byte[] enc = EncryptBody(bodyDecrypted, 0, bodyDecrypted.Length);
            // 加密内容写入最终buf
            buf.Put(enc);
            // 填充尾部
            PutTail(buf);
            // 回填
            PostFill(buf, pos);
        }
        /// <summary>
        /// 回填，有些字段必须填完整个包才能确定其内容，比如长度字段，那么这个方法将在
        /// 尾部填充之后调用
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="startPos">The start pos.</param>
        public void PostFill(ByteBuffer buf, int startPos)
        {
            // 如果是tcp包，到包的开头处填上包长度，然后回到目前的pos
            if (!user.IsUdp)
            {
                int len = buf.Length - startPos;
                buf.PutUShort(startPos, (ushort)len);
            }
        }

        /// <summary>
        /// 得到包体的字节数组
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="length">包总长度</param>
        /// <returns>包体字节数组</returns>
        protected byte[] GetBodyBytes(int length)
        {
            ByteBuffer buf = new ByteBuffer(QQGlobal.QQ_PACKET_MAX_SIZE);
            // 得到包体长度
            int bodyLen = length - QQGlobal.QQ_LENGTH_BASIC_FAMILY_OUT_HEADER - QQGlobal.QQ_LENGTH_BASIC_FAMILY_TAIL;
            if (!user.IsUdp) bodyLen -= 2;
            // 得到加密的包体内容
            byte[] body = buf.GetByteArray(bodyLen);
            return body;
        }


        /// <summary>
        /// 带表情消息
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static byte[] ConstructMessage(string Message)
        {
            ByteBuffer buf = new ByteBuffer();
            Regex r = new Regex(@"([^\[]+)*(\[face\d+\.gif\])([^\[]+)*");
            if (r.IsMatch(Message))
            {
                var Faces = r.Matches(Message);
                for (var i = 0; i < Faces.Count; i++)
                {
                    var face = Faces[i];
                    for (var j = 1; j < face.Groups.Count; j++)
                    {
                        var group = face.Groups[j].Value;
                        if (group.Contains("[face") && group.Contains(".gif]"))
                        {
                            var faceIndex = Convert.ToByte(group.Substring(5, group.Length - group.LastIndexOf(".") - 4));
                            if (faceIndex > 199)
                                faceIndex = 0;
                            //表情
                            buf.Put(new byte[] { 0x02, 0x00, 0x14, 0x01, 0x00, 0x01 });
                            buf.Put(faceIndex);
                            buf.Put(new byte[] { 0xFF, 0x00, 0x02, 0x14 });
                            buf.Put((byte)(faceIndex + 65));
                            buf.Put(new byte[] { 0x0B, 0x00, 0x08, 0x00, 0x01, 0x00, 0x04, 0x52, 0xCC, 0x85, 0x50 });
                        }
                        else if (!string.IsNullOrEmpty(group))
                        {
                            var GroupMsg = Encoding.UTF8.GetBytes(group);
                            //普通消息
                            ConstructMessage(buf, GroupMsg);
                        }
                    }
                }
            }
            return buf.ToByteArray();
        }
        /// <summary>
        /// 普通消息
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="GroupMsg"></param>
        public static void ConstructMessage(ByteBuffer buf, byte[] GroupMsg)
        {
            buf.Put(new byte[] { 0x01 });
            buf.PutUShort((ushort)(GroupMsg.Length + 3));
            buf.Put(new byte[] { 0x01 });
            buf.PutUShort((ushort)GroupMsg.Length);
            buf.Put(GroupMsg);
        }


        public static void SendAudio(uint QQ, string file)
        {

        }


        public static void SendOfflineFile(uint QQ, string file)
        {

        }
        /// <summary>
        /// XML消息组装
        /// </summary>
        /// <param name="buf">报文</param>
        /// <param name="_DateTime">时间</param>
        /// <param name="compressMsg">压缩消息数组</param>
        public static void SendXML(ByteBuffer buf, long _DateTime, byte[] compressMsg)
        {
            buf.PutLong(_DateTime);
            buf.Put(Util.RandomKey(4));
            buf.Put(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00 });
            buf.Put(new byte[] { 0x00, 0x0C });
            buf.Put(new byte[] { 0xE5, 0xBE, 0xAE, 0xE8, 0xBD, 0xAF, 0xE9, 0x9B, 0x85, 0xE9, 0xBB, 0x91 });
            buf.Put(new byte[] { 0x00, 0x00, 0x14 });
            buf.Put(0x01);
            buf.PutUShort((ushort)(compressMsg.Length + 11));
            buf.Put(0x01);
            buf.PutUShort((ushort)(compressMsg.Length + 1));
            buf.Put(0x01);
            buf.Put(compressMsg);
            buf.Put(new byte[] { 0x02, 0x00, 0x04, 0x00, 0x00, 0x00, 0x4D });
        }

        public static List<byte[]> SendJson(string Message)
        {
            List<byte[]> list = new List<byte[]>();
            byte[] buffer = Encoding.UTF8.GetBytes(Message);
            ByteBuffer byteBuffer = new ByteBuffer();
            byteBuffer.Put(1);
            byteBuffer.Put(buffer);
            byte[] array = byteBuffer.ToByteArray();
            byteBuffer = new ByteBuffer();
            byteBuffer.Put(25);
            byteBuffer.PutUShort((ushort)(array.Length + 3));
            byteBuffer.Put(1);
            byteBuffer.Put(array);
            list.Add(byteBuffer.ToByteArray());
            return list;
        }
    }
}
