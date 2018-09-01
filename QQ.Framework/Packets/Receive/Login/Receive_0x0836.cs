using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Receive.Login
{
    public class Receive_0x0836 : ReceivePacket
    {
        public byte[] VerifyCode { get; set; }
        public byte VerifyCommand { get; set; } = 0x01;
        public byte DataHead { get; set; }

        public Receive_0x0836(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_PACKET_TgtgtKey)
        {
        }

        protected override void ParseBody()
        {
            byte[] CipherText2 = QQTea.Decrypt(buffer, (int) reader.BaseStream.Position,
                (int) (buffer.Length - reader.BaseStream.Position - 1), user.QQ_SHARE_KEY);
            if (CipherText2 == null)
            {
                throw new Exception($"包内容解析出错，抛弃该包: {ToString()}");
            }

            if (GetPacketLength() == 871)
            {
                bodyDecrypted = CipherText2;
                reader = new BinaryReader(new MemoryStream(bodyDecrypted));
                reader.ReadBytes(20);
                user.QQ_PACKET_00BAVerifyToken = reader.ReadBytes(reader.BEReadChar());
                VerifyCode = reader.ReadBytes(reader.BEReadChar());
                VerifyCommand = reader.ReadByte();
                if (VerifyCommand == 0x00)
                    VerifyCommand = reader.ReadByte();
                user.QQ_PACKET_00BAVerifyCode = VerifyCode;
                user.QQ_PACKET_00BAToken = reader.ReadBytes(reader.BEReadChar());
                reader.ReadBytes(reader.BEReadChar());
            }
            else
            {
                bodyDecrypted = QQTea.Decrypt(CipherText2, _secretKey);
                if (bodyDecrypted == null)
                {
                    throw new Exception($"包内容解析出错，抛弃该包: {ToString()}");
                }

                //提取数据
                reader = new BinaryReader(new MemoryStream(bodyDecrypted));
                DataHead = reader.ReadByte();
                if (GetPacketLength() == 271 || GetPacketLength() == 207)
                {
                    reader.BEReadChar();
                    user.QQ_PACKET_TgtgtKey = reader.ReadBytes(reader.BEReadChar());
                    reader.BEReadChar();
                    user.QQ_tlv_0006_encr = reader.ReadBytes(reader.BEReadChar());
                    reader.ReadBytes(6);
                    if (GetPacketLength() == 271)
                    {
                        user.QQ_0836Token = reader.ReadBytes(reader.BEReadChar());
                    }

                    reader.BEReadChar();
                    reader.ReadBytes(reader.BEReadChar());
                }
                else if (GetPacketLength() > 700)
                {
                    reader.ReadBytes(6);
                    user.QQ_0828_rec_ecr_key = reader.ReadBytes(0x10);
                    reader.BEReadChar();
                    user.QQ_0836_038Token = reader.ReadBytes(0x38);
                    reader.ReadBytes(60);
                    var Judge = reader.ReadBytes(2);
                    var MsgLength = 0;
                    if (Util.ToHex(Judge) == "01 07")
                    {
                        MsgLength = 0;
                    }
                    else if (Util.ToHex(Judge) == "00 33")
                    {
                        MsgLength = 28;
                    }
                    else if (Util.ToHex(Judge) == "01 10")
                    {
                        MsgLength = 64;
                    }

                    reader.ReadBytes(28);
                    reader.ReadBytes(MsgLength);
                    user.QQ_0828_rec_decr_key = reader.ReadBytes(0x10);
                    reader.BEReadChar();
                    user.QQ_0836_088Token = reader.ReadBytes(0x88);
                    reader.ReadBytes(159);
                    user.QQ_ClientKey = reader.ReadBytes(112);
                    reader.ReadBytes(28);
                    var nick_length = reader.ReadByte();
                    user.NickName = Encoding.UTF8.GetString(reader.ReadBytes(nick_length));
                    user.Gender = reader.ReadByte();
                    reader.ReadBytes(4);
                    user.Age = reader.ReadByte();
                    reader.ReadBytes(10);
                    reader.ReadBytes(0x10);
                }
            }
        }
    }
}