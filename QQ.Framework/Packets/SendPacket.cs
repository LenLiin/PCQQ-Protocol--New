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
        protected static char seq = (char) Util.Random.Next();

        public MemoryStream bodyStream;
        public BinaryWriter bodyWriter;

        public BinaryWriter writer = new BinaryWriter(new MemoryStream());

        public SendPacket()
        {
        }

        /// <summary>
        ///     构造一个指定参数的包
        /// </summary>
        public SendPacket(QQUser User)
            : base(User)
        {
            Version = QQGlobal.QQ_CLIENT_VERSION;
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
            return QQTea.Encrypt(buf, offset, length, _secretKey);
        }

        /// <summary>
        ///     将包头部转化为字节流, 写入指定的ByteBuffer对象.
        /// </summary>
        protected virtual void PutHeader()
        {
            writer.Write(QQGlobal.QQ_HEADER_BASIC_FAMILY);
            writer.BEWrite(Version);
            writer.BEWrite((ushort) Command);
            writer.BEWrite(Sequence);
            writer.BEWrite(user.QQ);
        }

        protected static char GetNextSeq()
        {
            seq++;
            // 为了兼容iQQ
            // iQQ把序列号的高位都为0，如果为1，它可能会拒绝，wqfox称是因为TX是这样做的
            seq &= (char) 0x7FFF;
            if (seq == 0)
            {
                seq++;
            }

            return seq;
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
            writer.Write(QQGlobal.QQ_HEADER_03_FAMILY);
        }

        /// <summary>
        ///     将整个包转化为字节流, 并返回其值。
        ///     可直接使用QQClient.Send(new Send_0x__().WriteData())。
        /// </summary>
        public byte[] WriteData()
        {
            //保存当前pos
            var pos = (int) writer.BaseStream.Position;
            //填充头部
            PutHeader();
            //填充包体
            bodyStream = new MemoryStream();
            bodyWriter = new BinaryWriter(bodyStream);
            PutBody();
            //需要加密的包体
            bodyDecrypted = bodyStream.ToArray();
            var enc = EncryptBody(bodyDecrypted, 0, bodyDecrypted.Length);
            // 加密内容写入最终buf
            writer.Write(enc);
            // 填充尾部
            PutTail();
            // 回填
            PostFill(pos);
            return writer.BaseStream.ToBytesArray();
        }

        /// <summary>
        ///     回填，有些字段必须填完整个包才能确定其内容，比如长度字段，那么这个方法将在
        ///     尾部填充之后调用
        /// </summary>
        /// <param name="startPos">The start pos.</param>
        public void PostFill(int startPos)
        {
            // 如果是tcp包，到包的开头处填上包长度，然后回到目前的pos
            if (!user.IsUdp)
            {
                var len = (int) (writer.BaseStream.Length - startPos);
                var currentPos = writer.BaseStream.Position;
                writer.BaseStream.Position = startPos;
                writer.BEWrite((ushort) len);
                writer.BaseStream.Position = currentPos;
            }
        }

        /// <summary>
        ///     得到包体的字节数组
        /// </summary>
        /// <param name="length">包总长度</param>
        /// <returns>包体字节数组</returns>
        protected byte[] GetBodyBytes(int length)
        {
            var buf = new byte[QQGlobal.QQ_PACKET_MAX_SIZE];
            // 得到包体长度
            var bodyLen = length - QQGlobal.QQ_LENGTH_BASIC_FAMILY_OUT_HEADER - QQGlobal.QQ_LENGTH_BASIC_FAMILY_TAIL;
            if (!user.IsUdp)
            {
                bodyLen -= 2;
            }

            // 得到加密的包体内容
            // 没看懂，这个buf根本没被赋值，怎么读取的
            // byte[] body = buf.ReadBytes(bodyLen);
            return null;
        }

        public static void SendAudio(uint QQ, string file)
        {
        }


        public static void SendOfflineFile(uint QQ, string file)
        {
        }

        public static byte[] SendJson(string Message)
        {
            throw new Exception();
        }
    }
}