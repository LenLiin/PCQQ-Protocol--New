using System;
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
        internal long body_len;

        public BinaryWriter buf;

        /// <summary>
        ///     tlv命令
        /// </summary>
        internal ushort cmd;

        /// <summary>
        ///     包头长度
        /// </summary>
        internal int head_len = 4;

        public BaseTLV()
        {
            buf = new BinaryWriter(new MemoryStream());
        }

        /// <summary>
        ///     TLV名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     TLV版本
        /// </summary>
        public ushort wSubVer { get; set; }

        public void fill_body(byte[] bufdata, long length)
        {
            buf.BEWrite((ushort)length);
            buf.Write(bufdata);
        }

        public void fill_head(ushort cmd)
        {
            buf.BEWrite(cmd);
        }

        public byte[] get_buf()
        {
            return buf.BaseStream.ToBytesArray();
        }

       

        public void set_length()
        {
            
        }
    }
}