using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    internal class BaseTLV
    {
        private long _max = 128;

        /// <summary>
        ///     包长度
        /// </summary>
        internal long BodyLength;

        protected readonly BinaryWriter _buffer;

        /// <summary>
        ///     tlv命令
        /// </summary>
        internal ushort Command;

        /// <summary>
        ///     包头长度
        /// </summary>
        internal int HeadLength = 4;

        public BaseTLV()
        {
            _buffer = new BinaryWriter(new MemoryStream());
        }

        /// <summary>
        ///     TLV名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     TLV版本
        /// </summary>
        public ushort WSubVer { get; set; }

        protected void FillBody(byte[] bufdata, long length)
        {
            _buffer.BeWrite((ushort) length);
            _buffer.Write(bufdata);
        }

        protected void FillHead(ushort cmd)
        {
            _buffer.BeWrite(cmd);
        }

        protected byte[] GetBuffer()
        {
            return _buffer.BaseStream.ToBytesArray();
        }


        protected void SetLength()
        {
        }
    }
}