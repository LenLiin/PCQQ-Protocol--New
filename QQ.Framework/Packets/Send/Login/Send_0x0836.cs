using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Send.Login
{
    public class Send_0x0836 : SendPacket
    {
        /// <summary>
        /// 数据包类型默认为第一种
        /// </summary>
        Login0x0836Type _type = Login0x0836Type.Login0x0836_622;
        bool isVerify { get; set; } = false;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="byteBuffer"></param>
        /// <param name="User"></param>
        /// <param name="type">包类型</param>
        /// <param name="Key">数据包密钥</param>
        public Send_0x0836(QQUser User, Login0x0836Type type, bool isVerify = false)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SHARE_KEY;
            _type = type;
            Command = QQCommand.Login0x0836;
            this.isVerify = isVerify;
        }
        protected override void PutHeader(ByteBuffer buf)
        {
            base.PutHeader(buf);
            buf.Put(user.QQ_PACKET_FIX1);
            buf.Put(user.QQ_PUBLIC_KEY);
            buf.Put(new byte[] { 0x00, 0x00, 0x00, 0x10 });
            buf.Put(user.QQ_PACKET_0836_KEY1);
        }
        /// <summary>
        /// 初始化包体
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutBody(ByteBuffer buf)
        {
            user.QQ_tlv_001A_encr = QQTea.Encrypt(user.QQ_PACKET_FIX2, user.QQ_PACKET_TgtgtKey);
            if (_type == Login0x0836Type.Login0x0836_622 || isVerify)
            {
                user.QQ_tlv_0006_encr = GET_TLV_0006(user.LoginTime, user.ServerIp);
            }
            buf.Put(new byte[] { 0x01, 0x12 });
            buf.Put(new byte[] { 0x00, 0x38 });
            buf.Put(user.QQ_0825Token);
            buf.Put(new byte[] { 0x03, 0x0F });
            buf.Put(new byte[] { 0x00 });
            buf.Put((byte)(Util.HexStringToByteArray(Util.ConvertStringToHex(user.PcName)).Length + 2));
            buf.Put(new byte[] { 0x00 });
            buf.Put((byte)Util.HexStringToByteArray(Util.ConvertStringToHex(user.PcName)).Length);
            buf.Put(Util.HexStringToByteArray(Util.ConvertStringToHex(user.PcName)));
            buf.Put(new byte[] { 0x00, 0x05, 0x00, 0x06, 0x00, 0x02 });
            buf.PutLong(user.QQ);
            buf.Put(new byte[] { 0x00, 0x06 });
            buf.Put(new byte[] { 0x00, 0x78 });
            buf.Put(user.QQ_tlv_0006_encr);
            buf.Put(user.QQ_PACKET_FIX2);
            buf.Put(new byte[] { 0x00, 0x1A });
            buf.Put(new byte[] { 0x00, 0x40 });
            buf.Put(user.QQ_tlv_001A_encr);
            buf.Put(user.QQ_PACKET_0825DATA0);
            buf.Put(user.QQ_PACKET_0825DATA2);
            buf.PutLong(user.QQ);
            buf.Put(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            buf.Put(new byte[] { 0x01, 0x03 });
            buf.Put(new byte[] { 0x00, 0x14 });
            buf.Put(new byte[] { 0x00, 0x01 });
            buf.Put(new byte[] { 0x00, 0x10 });
            buf.Put(new byte[] { 0x60, 0xC9, 0x5D, 0xA7, 0x45, 0x70, 0x04, 0x7F, 0x21, 0x7D, 0x84, 0x50, 0x5C, 0x66, 0xA5, 0xC6 });

            if (_type == Login0x0836Type.Login0x0836_686)
            {
                buf.Put(new byte[] { 0x01, 0x10 });
                buf.Put(new byte[] { 0x00, 0x3C });
                buf.Put(new byte[] { 0x00, 0x01 });
                buf.Put(new byte[] { 0x00, 0x38 });
                buf.Put(user.QQ_0836Token);
            }

            buf.Put(new byte[] { 0x03, 0x12 });
            buf.Put(new byte[] { 0x00, 0x05 });
            buf.Put(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x01 });
            buf.Put(new byte[] { 0x05, 0x08 });
            buf.Put(new byte[] { 0x00, 0x05 });
            buf.Put(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00 });
            buf.Put(new byte[] { 0x03, 0x13 });
            buf.Put(new byte[] { 0x00, 0x19 });
            buf.Put(new byte[] { 0x01 });
            buf.Put(new byte[] { 0x01, 0x02 });
            buf.Put(new byte[] { 0x00, 0x10 });
            buf.Put(new byte[] { 0x04, 0xEA, 0x78, 0xD1, 0xA4, 0xFF, 0xCD, 0xCC, 0x7C, 0xB8, 0xD4, 0x12, 0x7D, 0xBB, 0x03, 0xAA });//两次0836包相同
            buf.Put(new byte[] { 0x00, 0x00, 0x00 });
            buf.Put(new byte[] { 0x00 });//可能为00,0F,1F
            buf.Put(new byte[] { 0x01, 0x02 });
            buf.Put(new byte[] { 0x00, 0x62 });
            buf.Put(new byte[] { 0x00, 0x01 });
            buf.Put(new byte[] { 0x04, 0xEB, 0xB7, 0xC1, 0x86, 0xF9, 0x08, 0x96, 0xED, 0x56, 0x84, 0xAB, 0x50, 0x85, 0x2E, 0x48 });//两次0836包不同
            buf.Put(new byte[] { 0x00, 0x38 });
            buf.Put(new byte[] { 0xE9, 0xAA, 0x2B, 0x4D, 0x26, 0x4C, 0x76, 0x18, 0xFE, 0x59, 0xD5, 0xA9, 0x82, 0x6A, 0x0C, 0x04, 0xB4, 0x49, 0x50, 0xD7, 0x9B, 0xB1, 0xFE, 0x5D, 0x97, 0x54, 0x8D, 0x82, 0xF3, 0x22, 0xC2, 0x48, 0xB9, 0xC9, 0x22, 0x69, 0xCA, 0x78, 0xAD, 0x3E, 0x2D, 0xE9, 0xC9, 0xDF, 0xA8, 0x9E, 0x7D, 0x8C, 0x8D, 0x6B, 0xDF, 0x4C, 0xD7, 0x34, 0xD0, 0xD3 });
            buf.Put(new byte[] { 0x00, 0x14 });
            buf.Put(user.QQ_PACKET_Crc32_Code);
            buf.PutULong(CRC32cs.GetCRC32Str(Util.ToHex(user.QQ_PACKET_Crc32_Code)));
        }
        public byte[] GET_TLV_0006(byte[] m_loginTime, byte[] m_loginIP)
        {
            ByteBuffer buf = new ByteBuffer();
            buf.Put(Util.RandomKey(4));
            buf.Put(new byte[] { 0x00, 0x02 });
            buf.PutLong(user.QQ);
            buf.Put(user.QQ_PACKET_0825DATA2);
            buf.Put(new byte[] { 0x00, 0x00, 0x01 });
            buf.Put(user.MD51);
            buf.Put(m_loginTime);
            buf.Put(new byte[] { 0x00 });//下面有时间时为01
            buf.Put(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            buf.Put(m_loginIP);
            buf.Put(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            buf.Put(new byte[] { 0x00, 0x10 });
            buf.Put(new byte[] { 0x15, 0x74, 0xC4, 0x89, 0x85, 0x7A, 0x19, 0xF5, 0x5E, 0xA9, 0xC9, 0xA3, 0x5E, 0x8A, 0x5A, 0x9B });
            buf.Put(user.QQ_PACKET_TgtgtKey);
            return QQTea.Encrypt(buf.ToByteArray(),user.Md52());
        }
    }
    public enum Login0x0836Type
    {
        Login0x0836_622 = 1,
        Login0x0836_686 = 2,
        Login0x0836_710 = 3
    }
}
