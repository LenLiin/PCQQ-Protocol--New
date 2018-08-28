using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets
{
    public class ReceivePacket : Packet
    {
        public ReceivePacket() : base()
        {

        }
        /// <summary>
        /// 包体长度
        /// </summary>
        /// <returns></returns>
        public int GetPacketLength (){
            return bodyEcrypted.Length;
        }
        /// <summary>
        /// 构造一个指定参数的包
        /// </summary>
        /// <param name="byteBuffer"></param>
        /// <param name="User"></param>
        /// <param name="Key">解密Key</param>
        public ReceivePacket(ByteBuffer byteBuffer, QQUser User,byte[] Key)
            : base(byteBuffer, User)
        {
            bodyEcrypted = byteBuffer.ToByteArray();
            //指定随包解密Key
            _secretKey = Key;
            //提取包头部分
            ParseHeader(byteBuffer);

            try
            {
                //解析包
                ParseBody(byteBuffer);
            }
            catch (Exception e)
            {
               user.MessageLog($"包内容解析出错,错误{e.Message}，包名: {ToString()}");
            }
            //提取包尾部分
            ParseTail(byteBuffer);
        }

        /// <summary>
        /// 从buf的当前位置解析包尾
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected void ParseTail(ByteBuffer buf)
        {
            buf.Get();
        }
        /// <summary>
        /// 解析包体，从buf的开头位置解析起
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected virtual void ParseBody(ByteBuffer buf)
        {

        }
        
        /// <summary>
        /// 从buf的当前位置解析包头
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected virtual void ParseHeader(ByteBuffer buf) {
            Header = buf.Get();
            Version = buf.GetChar();
            Command = (QQCommand)buf.GetUShort();
            Sequence = buf.GetChar();
            QQ = buf.GetInt();
            buf.GetByteArray(3);
        }
        public long QQ { get; set; }
    }
}
