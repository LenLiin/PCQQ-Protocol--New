using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Send.Message
{
    /// <summary>
    /// 发送好友消息
    /// </summary>
    public class Send_0x00CD : SendPacket
    {
        public Send_0x00CD(QQUser User, string Message, FriendMessageType messageType, long ToQQ)
           : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Message0x00CD;
            _message = Encoding.UTF8.GetBytes(Message);
            _messageType = messageType;
            _toQQ = ToQQ;
        }
        private byte _packetCount = 1;
        private byte _packetIndex = 0;
        /// <summary>
        /// 好友QQ
        /// </summary>
        long _toQQ;
        /// <summary>
        /// 消息类型
        /// </summary>
        public FriendMessageType _messageType { get; set; }
        private byte[] _message { get; set; }
        protected override void PutHeader(ByteBuffer buf)
        {
            base.PutHeader(buf);
            buf.Put(user.QQ_PACKET_FIXVER);
        }
        /// <summary>
        /// 初始化包体
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutBody(ByteBuffer buf)
        {
            var _DateTime = Util.GetTimeSeconds(DateTime.Now);
            if (_messageType == FriendMessageType.Shake)
            {
                buf.PutLong(user.QQ);
                buf.PutLong(_toQQ);
                buf.Put(new byte[] { 0x00, 0x00, 0x00, 0x00 });
                buf.Put(new byte[] { 0x37, 0x0F });
                buf.PutLong(user.QQ);
                buf.PutLong(_toQQ);
                buf.Put(Util.RandomKey());
                buf.Put(new byte[] { 0x00, 0xAF });
                buf.Put(Util.RandomKey(2));
                buf.PutLong(_DateTime);
                buf.Put(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                buf.Put(Util.RandomKey(4));
                buf.Put(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            }
            else
            {
                var _Md5 = user.QQ_SessionKey;
                buf.PutLong(user.QQ);
                buf.PutLong(_toQQ);
                buf.Put(new byte[] { 0x00, 0x00, 0x00, 0x0D, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x01,0x01 });
                buf.Put(new byte[] { 0x36, 0x43 });
                buf.PutLong(user.QQ);
                buf.PutLong(_toQQ);
                buf.Put(_Md5);
                buf.Put(new byte[] { 0x00, 0x0B });
                buf.Put(Util.RandomKey(2));
                buf.PutLong(_DateTime);
                buf.Put(new byte[] { 0x02, 0x34, 0x00, 0x00, 0x00, 0x00, _packetCount, _packetIndex, 0x00, 0x00, 0x01, 0x4D, 0x53, 0x47, 0x00, 0x00, 0x00, 0x00, 0x00 });
                buf.PutLong(_DateTime);
                buf.Put(Util.RandomKey(4));
                buf.Put(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00 });
                buf.Put(new byte[] { 0x00, 0x06 });
                buf.Put(new byte[] { 0xE5, 0xAE, 0x8B, 0xE4, 0xBD, 0x93 });
                buf.Put(new byte[] { 0x00, 0x00 });
                if (Encoding.UTF8.GetString(_message).Contains("[face") && Encoding.UTF8.GetString(_message).Contains(".gif]"))
                {
                    var MessageData = ConstructMessage(Encoding.UTF8.GetString(_message));
                    if (MessageData.Length != 0)
                    {
                        buf.Put(MessageData);
                    }
                }
                else
                {
                    //普通消息
                    ConstructMessage(buf, _message);
                }
            }
        }
        public static List<Send_0x00CD> SendLongMessage(QQUser User, string Message, FriendMessageType messageType, long ToQQ)
        {
            var buffer = new ByteBuffer();
            var list = new List<byte[]>();
            foreach (var chr in Message)
            {
                var bytes = Encoding.UTF8.GetBytes(chr.ToString());
                if (buffer.Length + bytes.Length > 699)
                {
                    list.Add(buffer.ToByteArray());
                    buffer.Initialize();
                }

                buffer.Put(bytes);
            }
            list.Add(buffer.ToByteArray());
            buffer.Initialize();

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

        public static void SendAudio(uint QQ, string file)
        {

        }


        public static void SendOfflineFile(uint QQ, string file)
        {

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
                            //face_add_65 ＝ DecToHex (到整数 (取文本中间 (msg_part, 6, 取文本长度 (msg_part) － 9)) ＋ 65)
                            buf.Put((byte)(faceIndex+65));//face_add_65
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
        private static void ConstructMessage(ByteBuffer buf, byte[] GroupMsg)
        {
            buf.Put(new byte[] { 0x01 });
            buf.PutUShort((ushort)(GroupMsg.Length + 3));
            buf.Put(new byte[] { 0x01 });
            buf.PutUShort((ushort)GroupMsg.Length);
            buf.Put(GroupMsg);
        }

        public static List<byte[]> SendXML(string Message)
        {
            List<byte[]> list = new List<byte[]>();
            byte[] buffer = Encoding.UTF8.GetBytes(Message.Trim());
            ByteBuffer byteBuffer = new ByteBuffer();
            byteBuffer.Put(1);
            byteBuffer.Put(buffer);
            byte[] token = byteBuffer.ToByteArray();
            byteBuffer = new ByteBuffer();
            byteBuffer.Put(1);
            byteBuffer.Put(token);
            byteBuffer.Put(new byte[7]{ 2,0, 4,0,0,0,1});
            token = byteBuffer.ToByteArray();
            byteBuffer = new ByteBuffer();
            byteBuffer.Put(20);
            byteBuffer.Put(token);
            list.Add(byteBuffer.ToByteArray());
            return list;
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