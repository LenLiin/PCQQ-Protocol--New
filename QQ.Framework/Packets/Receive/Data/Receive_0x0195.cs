using System.Collections.Generic;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Receive.Data
{
    public class Receive_0X0195 : ReceivePacket
    {
        /// <summary>
        ///     群分组信息查询
        /// </summary>
        public List<string> GroupCategory = new List<string>();

        /// <summary>
        ///     群分组信息查询
        /// </summary>
        public Receive_0X0195(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
            Reader.BeReadUInt16();
            var groupCategoryLength = Reader.BeReadUInt16();
            Reader.ReadBytes(3);
            Reader.ReadByte();
            Reader.BeReadUInt16();
            Reader.ReadByte();

            var itemLength = Reader.BeReadUInt16();
            while (itemLength > 0)
            {
                var item = Reader.ReadBytes(itemLength);

                var itemReader = new BinaryReader(new MemoryStream(item));
                itemReader.ReadByte();
                var indnex = itemReader.ReadByte();
                var cateName = Util.GetString(itemReader.ReadBytes(itemReader.ReadByte()));
                GroupCategory.Add(cateName);
                User.MessageLog($"群分组{indnex}：{cateName}");

                itemLength = Reader.BeReadUInt16();
            }
        }
    }
}