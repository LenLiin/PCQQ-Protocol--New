using QQ.Framework.Utils;
using System;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class BaseTLV
    {
        /// <summary>
        /// TLV名称
        /// </summary>
        public string Name { get;  set; }
        /// <summary>
        /// TLV版本
        /// </summary>
        public ushort wSubVer { get;  set; }
        long _max = 128;
        /// <summary>
        /// 包长度
        /// </summary>
        internal long body_len = 0;
        internal byte[] buf = null;
        /// <summary>
        /// tlv命令
        /// </summary>
        internal int cmd = 0;
        /// <summary>
        /// 包头长度
        /// </summary>
        internal int head_len = 4;
        long pos = 0;
        public BaseTLV()
        {
            buf = new byte[this._max];
        }

        public void fill_body(byte[] bufdata, long length)
        {
            if (length > this._max - this.head_len)
            {
                this._max = (64 + (length + this.head_len));
                byte[] bufdata1 = new byte[this._max];
                Array.Copy(this.buf, 0, bufdata1, 0, this.pos);
                this.buf = bufdata1;
            }
            this.body_len = length;
            Array.Copy(bufdata, 0, this.buf, this.pos, length);
            this.pos += length;
        }

        public void fill_head(int cmd)
        {
            Util.int16_to_buf(this.buf, this.pos, cmd);
            this.pos += 2;
            Util.int16_to_buf(this.buf, this.pos, 0);
            this.pos += 2;
        }

        public byte[] get_buf()
        {
            byte[] bufdata = new byte[this.pos];
            Array.Copy(this.buf, 0, bufdata, 0, this.pos);
            return bufdata;
        }

        public byte[] get_data()
        {
            byte[] bufdata = new byte[this.body_len];
            Array.Copy(this.buf, this.head_len, bufdata, 0, this.body_len);
            return bufdata;
        }

        public long get_data_len()
        {
            return this.body_len;
        }

        public void set_buf(byte[] bufdata, int length)
        {
            if (length > this._max)
            {
                this._max = length + 128;
                this.buf = new byte[this._max];
            }
            this.pos = length;
            Array.Copy(bufdata, 0, this.buf, 0, length);
            this.cmd = Util.buf_to_int16(bufdata, 0);
            this.body_len = length - this.head_len;
        }

        public void set_buf(byte[] bufdata, int index, int length)
        {
            if (length > this._max)
            {
                this._max = (length + 128);
                this.buf = new byte[this._max];
            }
            this.pos = length;
            Array.Copy(bufdata, index, this.buf, 0, length);
            this.cmd = Util.buf_to_int16(bufdata, index);
            this.body_len = length - this.head_len;
        }

        public void set_data(byte[] bufdata, long length)
        {
            if (length + this.head_len > this._max)
            {
                this._max = 128 + (length + this.head_len);
                byte[] bufdata1 = new byte[this._max];
                Array.Copy(this.buf, 0, bufdata1, 0, this.head_len);
                this.buf = bufdata1;
            }
            this.pos = length + this.head_len;
            Array.Copy(bufdata, 0, this.buf, this.head_len, length);
            this.body_len = length;
            Util.int16_to_buf(this.buf, 0, this.cmd);
            Util.int16_to_buf(this.buf, 2, this.body_len);
        }

        public void set_length()
        {
            Util.int16_to_buf(this.buf, 2, this.pos - this.head_len);
        }
    }
}
