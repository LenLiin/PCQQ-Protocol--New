using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Receive.Message
{
    /// <summary>
    /// </summary>
    public class Receive_0X0388 : ReceivePacket
    {
        /// <summary>
        /// </summary>
        public Receive_0X0388(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        public string _30Key { get; set; }
        public string _48Key { get; set; }
        private byte[] _md5 { get; set; }

        public string Md5
        {
            get
            {
                return Util.ToHex(_md5).Replace(" ", "");
            }
        }
        private byte[] _ukey { get; set; }

        public string Ukey
        {
            get
            {
                return Util.ToHex(_ukey).Replace(" ", "");
            }
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
            Reader.ReadBytes(4);
            var DataLength = Reader.ReadBytes(4);
            Reader.ReadBytes(5);
            Reader.ReadByte();
            Reader.ReadBytes(5);
            Reader.ReadBytes(3);
            Reader.ReadByte();
            if (Reader.ReadByte() == 0x08)
            {
                Reader.ReadBytes(8);
                _md5 = Reader.ReadBytes(Reader.ReadByte());
                Reader.ReadBytes(3);
                Reader.ReadBytes(3);//size
                Reader.ReadByte();
                Reader.ReadUInt16();//width
                Reader.ReadByte();
                Reader.ReadUInt16();//height
                Reader.ReadByte();
                _30Key = Util.Length_toPB(Util.ToHex(Reader.ReadBytes(5)).Replace(" ", ""));
                Reader.ReadByte();
                Reader.ReadByte();
                Reader.ReadByte();
                _48Key = Util.Length_toPB(Util.ToHex(Reader.ReadBytes(5)).Replace(" ", ""));
            }
            else
            {
                Reader.ReadBytes(7);
                _30Key = Util.Length_toPB(Util.ToHex(Reader.ReadBytes(5)).Replace(" ", ""));
                Reader.ReadBytes(32);
                _ukey = Reader.ReadBytes(128);
                Reader.ReadByte();
                _48Key = Util.Length_toPB(Util.ToHex(Reader.ReadBytes(5)).Replace(" ", ""));
            }
        }
    }
}