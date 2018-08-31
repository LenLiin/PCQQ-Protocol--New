using System;


namespace System
{
    // <summary>
    /// 创建一个可变长的Byte数组方便Push数据和Pop数据
    /// 数组的最大长度为1024,超过会产生溢出
    /// 数组的最大长度由常量MAX_LENGTH设定
    /// 
    /// 注:由于实际需要,可能要从左到右取数据,所以这里
    /// 定义的Pop函数并不是先进后出的函数,而是从0开始.
    /// 
    /// @Author: Red_angelX
    /// </summary>
    public class ByteBuffer
    {
        /// <summary>
        /// 数组的最大长度
        /// </summary>
        private int MAX_LENGTH = QQ.Framework.QQGlobal.QQ_PACKET_MAX_SIZE;

        /// <summary>
        /// 固定长度的中间数组
        /// </summary>
        private byte[] TEMP_BYTE_ARRAY;


        /// <summary>
        /// 当前数组长度
        /// </summary>
        private int CURRENT_LENGTH = 0;

        /// <summary>
        /// 当前Pop指针位置
        /// </summary>
        private int CURRENT_POSITION = 0;

        /// <summary>
        /// 最后返回数组
        /// </summary>
        private byte[] RETURN_ARRAY;

        /**/
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ByteBuffer()
        {
            this.Initialize();
        }
        public ByteBuffer(int length)
        {
            this.MAX_LENGTH = length;
            this.Initialize();
        }
        /**/
        /// <summary>
        /// 重载的构造函数,用一个Byte数组来构造
        /// </summary>
        /// <param name="bytes">用于构造ByteBuffer的数组</param>
        public ByteBuffer(byte[] bytes)
        {
            this.Initialize();
            this.Put(bytes);
        }


        /**/
        /// <summary>
        /// 获取当前ByteBuffer的长度
        /// 当用于接收网络数据流的时候，这个属性必须是可以被赋值的。
        /// </summary>
        public int Length
        {
            get
            {
                return CURRENT_LENGTH;
            }
            set
            {
                CURRENT_LENGTH = value;
            }
        }

        /**/
        /// <summary>
        /// 获取/设置当前出栈指针位置
        /// </summary>
        public int Position
        {
            get
            {
                return CURRENT_POSITION;
            }
            set
            {
                CURRENT_POSITION = value;
            }
        }

        /**/
        /// <summary>
        /// 获取ByteBuffer所生成的数组
        /// 长度必须小于 [MAXSIZE]
        /// </summary>
        /// <returns>Byte[]</returns>
        public byte[] ToByteArray()
        {
            //分配大小
            RETURN_ARRAY = new byte[CURRENT_LENGTH];
            //调整指针
            Array.Copy(TEMP_BYTE_ARRAY, 0, RETURN_ARRAY, 0, CURRENT_LENGTH);
            return RETURN_ARRAY;
        }

        /**/
        /// <summary>
        /// 初始化ByteBuffer的每一个元素,并把当前指针指向头一位
        /// </summary>
        public void Initialize()
        {
            TEMP_BYTE_ARRAY = new byte[MAX_LENGTH];
            TEMP_BYTE_ARRAY.Initialize();
            CURRENT_LENGTH = 0;
            CURRENT_POSITION = 0;
        }

        /**/
        /// <summary>
        /// 向ByteBuffer压入一个字节
        /// </summary>
        /// <param name="by">一位字节</param>
        public void Put(byte by)
        {
            Put(CURRENT_LENGTH, by);
            CURRENT_LENGTH += 1;
        }
        /// <summary>
        /// 在指定位置压入一个字节，Position不变
        /// 	<remark>2008-02-19 </remark>
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="by">The by.</param>
        public void Put(int index, byte by)
        {
            TEMP_BYTE_ARRAY[index] = by;
        }

        /**/
        /// <summary>
        /// 向ByteBuffer压入数组
        /// </summary>
        /// <param name="ByteArray">数组</param>
        public void Put(byte[] ByteArray)
        {
            //把自己CopyTo目标数组
            ByteArray.CopyTo(TEMP_BYTE_ARRAY, CURRENT_LENGTH);
            //调整长度
            CURRENT_LENGTH += ByteArray.Length;
        }
        /// <summary>
        /// 	<remark>2008-02-29 </remark>
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        public void Put(byte[] byteArray, int offset, int length)
        {
            byte[] temp = new byte[length];
            Array.Copy(byteArray, offset, temp, 0, length);
            Put(temp);
        }
        /**/
        /// <summary>
        /// 向ByteBuffer压入两字节的Short
        /// </summary>
        /// <param name="Num">2字节Short</param>
        public void PutUShort(UInt16 Num)
        {
            PutUShort(CURRENT_LENGTH, Num);
            CURRENT_LENGTH += 2;
            //TEMP_BYTE_ARRAY[CURRENT_LENGTH++] = (byte)(((Num & 0xff00) >> 8) & 0xff);
            //TEMP_BYTE_ARRAY[CURRENT_LENGTH++] = (byte)((Num & 0x00ff) & 0xff);
        }
        /// <summary>
        /// 在指定位置压入两个字节，Position不变
        /// 	<remark>2008-02-19 </remark>
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="num">The num.</param>
        public void PutUShort(int index, UInt16 Num)
        {
            TEMP_BYTE_ARRAY[index] = (byte)(((Num & 0xff00) >> 8) & 0xff);
            TEMP_BYTE_ARRAY[index + 1] = (byte)((Num & 0x00ff) & 0xff);
        }

        /// <summary>
        /// 压入字符
        /// 
        /// </summary>
        /// <param name="c">The c.</param>
        public void PutChar(char c)
        {
            PutUShort((ushort)c);
        }
        /// <summary>
        /// 在指定位置压入两个字节，Position不变
        /// 	<remark>2008-02-19 </remark>
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="c">The c.</param>
        public void PutChar(int index, char c)
        {
            PutUShort(index, (ushort)c);
        }

        /**/
        /// <summary>
        /// 向ByteBuffer压入一个无符Int值
        /// </summary>
        /// <param name="Num">4字节UInt32</param>
        public void PutInt(UInt32 Num)
        {
            PutInt(CURRENT_LENGTH, Num);
            CURRENT_LENGTH += 4;
        }
        /// <summary>向ByteBuffer压入一个Int值
        /// 	<remark>2008-02-23 </remark>
        /// </summary>
        /// <param name="num">The num.</param>
        public void PutInt(int num)
        {
            PutInt((UInt32)num);
        }
        /// <summary>在指定位置压入4个字节，Position不变
        /// 	<remark>2008-02-19 </remark>
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="num">The num.</param>
        public void PutInt(int index, UInt32 Num)
        {
            TEMP_BYTE_ARRAY[index] = (byte)(((Num & 0xff000000) >> 24) & 0xff);
            TEMP_BYTE_ARRAY[index + 1] = (byte)(((Num & 0x00ff0000) >> 16) & 0xff);
            TEMP_BYTE_ARRAY[index + 2] = (byte)(((Num & 0x0000ff00) >> 8) & 0xff);
            TEMP_BYTE_ARRAY[index + 3] = (byte)((Num & 0x000000ff) & 0xff);
        }
        /**/
        /// <summary>
        /// 向ByteBuffer压入一个Long值
        /// </summary>
        /// <param name="Num">4字节Long</param>
        public void PutLong(long Num)
        {
            PutLong(CURRENT_LENGTH, Num);
            CURRENT_LENGTH += 4;
        }
        /// <summary>在指定位置压入4个字节，Position不变
        /// 	<remark>2008-02-19 </remark>
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="Num">The num.</param>
        public void PutLong(int index, long Num)
        {
            TEMP_BYTE_ARRAY[index] = (byte)(((Num & 0xff000000) >> 24) & 0xff);
            TEMP_BYTE_ARRAY[index + 1] = (byte)(((Num & 0x00ff0000) >> 16) & 0xff);
            TEMP_BYTE_ARRAY[index + 2] = (byte)(((Num & 0x0000ff00) >> 8) & 0xff);
            TEMP_BYTE_ARRAY[index + 3] = (byte)((Num & 0x000000ff) & 0xff);
        }
        /**/
        /// <summary>
        /// 向ByteBuffer压入一个Long值
        /// </summary>
        /// <param name="Num">4字节Long</param>
        public void PutULong(ulong Num)
        {
            PutULong(CURRENT_LENGTH, Num);
            CURRENT_LENGTH += 4;
        }
        /// <summary>在指定位置压入4个字节，Position不变
        /// 	<remark>2008-02-19 </remark>
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="Num">The num.</param>
        public void PutULong(int index, ulong Num)
        {
            TEMP_BYTE_ARRAY[index] = (byte)(((Num & 0xff000000) >> 24) & 0xff);
            TEMP_BYTE_ARRAY[index + 1] = (byte)(((Num & 0x00ff0000) >> 16) & 0xff);
            TEMP_BYTE_ARRAY[index + 2] = (byte)(((Num & 0x0000ff00) >> 8) & 0xff);
            TEMP_BYTE_ARRAY[index + 3] = (byte)((Num & 0x000000ff) & 0xff);
        }
        /**/
        /// <summary>
        /// 从指定的位置提取1个字节的byte，保持原有的位置不变
        /// </summary>
        /// <returns>1字节Byte</returns>
        public byte Get(int index)
        {
            byte ret = TEMP_BYTE_ARRAY[index];
            return ret;
        }
        /// <summary>从ByteBuffer的当前位置弹出一个Byte,并提升一位
        /// 	<remark>2008-02-20 </remark>
        /// </summary>
        /// <returns></returns>
        public byte Get()
        {
            return Get(CURRENT_POSITION++);
        }

        /**/
        /// <summary>
        /// 从指定的位置提取两个字节的Short，保持原有的位置不变
        /// </summary>
        /// <returns>2字节Short</returns>
        public UInt16 GetUShort(int index)
        {
            //溢出
            if (index + 1 >= CURRENT_LENGTH)
            {
                return 0;
            }
            UInt16 ret = (UInt16)(TEMP_BYTE_ARRAY[index] << 8 | TEMP_BYTE_ARRAY[index + 1]);
            return ret;
        }
        /// <summary>从ByteBuffer的当前位置弹出一个Short,并提升两位
        /// 	<remark>2008-02-20 </remark>
        /// </summary>
        /// <returns></returns>
        public UInt16 GetUShort()
        {
            UInt16 ret = GetUShort(CURRENT_POSITION);
            CURRENT_POSITION += 2;
            return ret;
        }
        public Int16 GetShort()
        {
            Int16 ret = (Int16)GetUShort(CURRENT_POSITION);
            CURRENT_POSITION += 2;
            return ret;
        }
        /// <summary>
        /// 弹出一个字符
        /// 
        /// </summary>
        /// <returns></returns>
        public Char GetChar()
        {
            return (char)GetUShort();
        }
        /// <summary>从指定的位置提取两个字节的char，保持原有的位置不变
        /// 	<remark>2008-02-20 </remark>
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public char GetChar(int index)
        {
            return (char)GetUShort(index);
        }

        /**/
        /// <summary>
        /// 从ByteBuffer的当前位置弹出一个uint,并提升4位
        /// </summary>
        /// <returns>4字节UInt</returns>
        public uint GetUInt()
        {
            uint ret = GetUInt(CURRENT_POSITION);
            CURRENT_POSITION += 4;
            return ret;
        }
        /// <summary>从ByteBuffer的当前位置弹出一个int,并提升4位
        /// 	<remark>2008-02-23 </remark>
        /// </summary>
        /// <returns></returns>
        public int GetInt()
        {
            return (int)GetUInt();
        }
        /// <summary>从指定的位置提取4个字节的uint，保持原有的位置不变
        /// 	<remark>2008-02-20 </remark>
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public uint GetUInt(int index)
        {
            if (index + 3 >= CURRENT_LENGTH)
                return 0;
            uint ret = (uint)(TEMP_BYTE_ARRAY[index] << 24 | TEMP_BYTE_ARRAY[index + 1] << 16 | TEMP_BYTE_ARRAY[index + 2] << 8 | TEMP_BYTE_ARRAY[index + 3]);
            return ret;
        }

        /**/
        /// <summary>
        /// 从ByteBuffer的当前位置弹出一个long,并提升4位
        /// </summary>
        /// <returns>4字节Long</returns>
        public long GetLong()
        {
            long ret = GetLong(CURRENT_POSITION);
            CURRENT_POSITION += 4;
            return ret;
        }
        /// <summary>从指定的位置提取4个字节的long，保持原有的位置不变
        /// 	<remark>2008-02-20 </remark>
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public long GetLong(int index)
        {
            if (index + 3 >= CURRENT_LENGTH)
                return 0;
            long ret = (long)(TEMP_BYTE_ARRAY[index] << 24 | TEMP_BYTE_ARRAY[index + 1] << 16 | TEMP_BYTE_ARRAY[index + 2] << 8 | TEMP_BYTE_ARRAY[index + 3]);
            return ret;
        }

        /**/
        /// <summary>
        /// 从ByteBuffer的当前位置弹出长度为Length的Byte数组,提升Length位
        /// </summary>
        /// <param name="Length">数组长度</param>
        /// <returns>Length长度的byte数组</returns>
        public byte[] GetByteArray(int Length)
        {
            //溢出
            if (CURRENT_POSITION + Length > CURRENT_LENGTH)
            {
                return new byte[0];
            }
            byte[] ret = new byte[Length];
            Array.Copy(TEMP_BYTE_ARRAY, CURRENT_POSITION, ret, 0, Length);
            //提升位置
            CURRENT_POSITION += Length;
            return ret;
        }

        /// <summary>
        /// 判断是否到结尾
        /// 	<remark>2008-02-20 </remark>
        /// </summary>
        /// <returns></returns>
        public bool HasRemaining()
        {
            return CURRENT_POSITION < CURRENT_LENGTH;
        }

        /// <summary>
        /// 剩余字节数
        /// 	<remark>2008-02-23 </remark>
        /// </summary>
        /// <returns></returns>
        public int Remaining()
        {
            return Length - Position;
        }
        /// <summary>重绕此缓冲区。将位置设置为 0 并丢弃标记。
        /// 	<remark>2008-02-26 </remark>
        /// </summary>
        public void Rewind()
        {
            Position = 0;
        }

        /// <summary>
        /// 返回用于存储的字节数组
        /// </summary>
        public byte[] ByteArray
        {
            get { return TEMP_BYTE_ARRAY; }
        }

        public override bool Equals(object obj)
        {
            if (obj is ByteBuffer)
            {                
                ByteBuffer buf = (ByteBuffer)obj;
                if (buf.Length != this.Length)
                    return false;                
                for (int i = 0; i < this.Length; i++)
                {
                    if (this.ByteArray[i] != buf.ByteArray[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        
        internal byte[] GetToken()
        {
            return GetByteArray(0x38);
        }

    }
}
