using System;
using QQ.Framework.Utils;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class BaseTLV
    {
        private long _max = 128;

        /// <summary>
        ///     包长度
        /// </summary>
        internal long body_len;

        internal byte[] buf;

        /// <summary>
        ///     tlv命令
        /// </summary>
        internal int cmd;

        /// <summary>
        ///     包头长度
        /// </summary>
        internal int head_len = 4;

        private long pos;

        public BaseTLV()
        {
            buf = new byte[_max];
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
            if (length > _max - head_len)
            {
                _max = 64 + length + head_len;
                var bufdata1 = new byte[_max];
                Array.Copy(buf, 0, bufdata1, 0, pos);
                buf = bufdata1;
            }

            body_len = length;
            Array.Copy(bufdata, 0, buf, pos, length);
            pos += length;
        }

        public void fill_head(int cmd)
        {
            Util.int16_to_buf(buf, pos, cmd);
            pos += 2;
            Util.int16_to_buf(buf, pos, 0);
            pos += 2;
        }

        public byte[] get_buf()
        {
            var bufdata = new byte[pos];
            Array.Copy(buf, 0, bufdata, 0, pos);
            return bufdata;
        }

        public byte[] get_data()
        {
            var bufdata = new byte[body_len];
            Array.Copy(buf, head_len, bufdata, 0, body_len);
            return bufdata;
        }

        public long get_data_len()
        {
            return body_len;
        }

        public void set_buf(byte[] bufdata, int length)
        {
            if (length > _max)
            {
                _max = length + 128;
                buf = new byte[_max];
            }

            pos = length;
            Array.Copy(bufdata, 0, buf, 0, length);
            cmd = Util.buf_to_int16(bufdata, 0);
            body_len = length - head_len;
        }

        public void set_buf(byte[] bufdata, int index, int length)
        {
            if (length > _max)
            {
                _max = length + 128;
                buf = new byte[_max];
            }

            pos = length;
            Array.Copy(bufdata, index, buf, 0, length);
            cmd = Util.buf_to_int16(bufdata, index);
            body_len = length - head_len;
        }

        public void set_data(byte[] bufdata, long length)
        {
            if (length + head_len > _max)
            {
                _max = 128 + length + head_len;
                var bufdata1 = new byte[_max];
                Array.Copy(buf, 0, bufdata1, 0, head_len);
                buf = bufdata1;
            }

            pos = length + head_len;
            Array.Copy(bufdata, 0, buf, head_len, length);
            body_len = length;
            Util.int16_to_buf(buf, 0, cmd);
            Util.int16_to_buf(buf, 2, body_len);
        }

        public void set_length()
        {
            Util.int16_to_buf(buf, 2, pos - head_len);
        }
    }
}