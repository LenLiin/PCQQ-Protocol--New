using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Receive.Data
{
    public class Receive_0x0195 : ReceivePacket
    {
        /// <summary>
        ///     群分组信息查询
        /// </summary>
        public Receive_0x0195(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.TXProtocol.SessionKey)
        {
        }
        /// <summary>
        /// 群分组信息查询
        /// </summary>
        public List<string> GroupCategory = new List<string>();
        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
            Reader.BeReadUInt16();
            var GroupCategoryLength = Reader.BeReadUInt16();
            Reader.ReadBytes(3);
            Reader.ReadByte();
            Reader.BeReadUInt16();
            Reader.ReadByte();
            Reader.ReadBytes(2);
            var Data = Util.ToHex(Reader.ReadBytes((int)(Reader.BaseStream.Length - Reader.BaseStream.Position - 2)));
            foreach (var item in Data.Replace("00 18", "_").Split('_'))
            {
                var ItemReader = new BinaryReader(new MemoryStream(Util.HexStringToByteArray(item.Trim())));
                ItemReader.ReadByte();
                var Indnex = ItemReader.ReadByte();
                var CateName = Util.GetString(ItemReader.ReadBytes(ItemReader.ReadByte()));
                GroupCategory.Add(CateName);
                User.MessageLog($"群分组{Indnex}：{CateName}");
            }
            Reader.ReadBytes(2);
        }
    }
}