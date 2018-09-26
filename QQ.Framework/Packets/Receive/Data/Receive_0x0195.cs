using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Receive.Data
{
    public class Receive_0X0195 : ReceivePacket
    {
        /// <summary>
        ///     群分组信息查询
        /// </summary>
        public Receive_0X0195(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
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
            var groupCategoryLength = Reader.BeReadUInt16();
            Reader.ReadBytes(3);
            Reader.ReadByte();
            Reader.BeReadUInt16();
            Reader.ReadByte();
            Reader.ReadBytes(2);
            var data = Util.ToHex(Reader.ReadBytes((int)(Reader.BaseStream.Length - Reader.BaseStream.Position - 2)));
            foreach (var item in data.Replace("00 18", "_").Split('_'))
            {
                var itemReader = new BinaryReader(new MemoryStream(Util.HexStringToByteArray(item.Trim())));
                itemReader.ReadByte();
                var indnex = itemReader.ReadByte();
                var cateName = Util.GetString(itemReader.ReadBytes(itemReader.ReadByte()));
                GroupCategory.Add(cateName);
                User.MessageLog($"群分组{indnex}：{cateName}");
            }
            Reader.ReadBytes(2);
        }
    }
}