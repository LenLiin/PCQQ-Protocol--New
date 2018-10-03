using System;

namespace QQ.Framework.Packets
{
    /// <summary>
    ///     数据包
    /// </summary>
    public abstract class Packet
    {
        /// <summary>
        ///     明文包体
        /// </summary>
        public byte[] BodyDecrypted;

        /// <summary>
        ///     密文包体
        /// </summary>
        public byte[] BodyEcrypted;

        /// <summary>
        ///     原始数据。对于接收的包，这个值为未解密的。
        /// </summary>
        public byte[] Buffer = new byte[QQGlobal.QQPacketMaxSize];

        /// <summary>
        ///     QQUser
        ///     为了支持一个JVM中创建多个QQClient，包中需要保持一个QQUser的引用以
        ///     确定包的用户相关字段如何填写
        ///     <remark>abu 2008-02-18 </remark>
        /// </summary>
        /// <value></value>
        public QQUser User;

        public Packet()
        {
            DateTime = DateTime.Now;
        }

        public Packet(QQUser user)
            : this()
        {
            User = user;
        }

        /// <summary>
        ///     构造一个指定参数的包
        /// </summary>
        public Packet(byte[] byteBuffer, QQUser user)
            : this(user)
        {
            Buffer = byteBuffer;
        }

        /// <summary>
        ///     加密密钥
        /// </summary>
        public byte[] SecretKey { get; set; }

        /// <summary>
        ///     包头字节
        /// </summary>
        public byte Header { get; set; }

        /// <summary>
        ///     版本标志
        /// </summary>
        public char Version { get; set; }

        /// <summary>
        ///     包命令, 如：0x0825
        /// </summary>
        /// <value></value>
        public QQCommand Command { get; set; }

        /// <summary>
        ///     包序号
        /// </summary>
        public char Sequence { get; set; }

        /// <summary>
        ///     包的接收时间或发送时间
        /// </summary>
        /// <value></value>
        public DateTime DateTime { get; set; }

        /// <summary>
        ///     标识这个包是哪个包
        /// </summary>
        /// <returns></returns>
        public QQCommand GetQQCommand()
        {
            return Command;
        }

        /// <summary>
        ///     密文的起始位置，这个位置是相对于包体的第一个字节来说的，如果这个包是未知包，
        ///     返回-1，这个方法只对某些协议族有意义
        /// </summary>
        /// <returns></returns>
        protected virtual int GetCryptographStart()
        {
            return -1;
        }

        /// <summary>
        ///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        ///     <see cref="T:System.Object" />.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.</param>
        /// <returns>
        ///     true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />;
        ///     otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj" /> parameter is null.</exception>
        public override bool Equals(object obj)
        {
            if (obj is Packet packet)
            {
                return Header == packet.Header && Command == packet.Command && Sequence == packet.Sequence;
            }

            return base.Equals(obj);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        public override int GetHashCode()
        {
            return Hash(Sequence, Command);
        }

        /// <summary>
        ///     得到hash值
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public static int Hash(char sequence, QQCommand command)
        {
            return (sequence << 16) | (ushort) command;
        }

        /// <summary>
        ///     包的描述性名称
        /// </summary>
        /// <returns></returns>
        public virtual string GetPacketName()
        {
            return "未知数据包";
        }
    }
}