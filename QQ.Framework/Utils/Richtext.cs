using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QQ.Framework.Utils
{
    public class Richtext
    {
        public List<TextSnippet> Snippets = new List<TextSnippet>();
        public int Length => Snippets.Sum(s => s.Length);

        public static Richtext Parse(BinaryReader reader)
        {
            var result = new Richtext();
            // TODO: 解析富文本
            try
            {
                var messageType = reader.ReadByte();
                var dataLength = reader.BeReadChar();
                var pos = reader.BaseStream.Position;
                while (pos + dataLength < reader.BaseStream.Length)
                {
                    reader.ReadByte();
                    switch (messageType)
                    {
                        case 0x01: //文本消息
                        {
                            var messageStr = Encoding.UTF8.GetString(reader.ReadBytes(reader.BeReadChar()));
                            if (messageStr.Contains("@"))
                            {
                                //Reader.ReadBytes(10);
                                //var AtQQ = Util.GetQQNumRetUint(Util.ToHex(Reader.ReadBytes(4)));//被At人的QQ号
                                result.Snippets.Add(new TextSnippet(messageStr, MessageType.At));
                            }
                            else
                            {
                                result.Snippets.Add(messageStr);
                            }

                            break;
                        }

                        case 0x02: //小黄豆表情
                        {
                            result.Snippets.Add(new TextSnippet(
                                Util.GetQQNumRetUint(Util.ToHex(reader.ReadBytes(reader.BeReadChar()))).ToString(),
                                MessageType.Emoji));
                            break;
                        }
                        case 0x03: //图片
                        {
                            result.Snippets.Add(new TextSnippet(
                                Encoding.UTF8.GetString(reader.ReadBytes(reader.BeReadChar())), MessageType.Picture));
                            break;
                        }
                        case 0x0A: //音频
                        {
                            result.Snippets.Add(new TextSnippet(Encoding.UTF8.GetString(reader.ReadBytes(reader.BeReadChar())), MessageType.Audio));
                            break;
                        }
                        case 0x0E: //未知
                        {
                            break;
                        }
                        case 0x14: //XML
                        {
                           reader.ReadByte();
                            result.Snippets.Add(new TextSnippet( GZipByteArray.DecompressString(reader.ReadBytes((int) (reader.BaseStream.Length - 1))), MessageType.Xml));
                            break;
                        }
                        case 0x18: //群文件
                        {
                            reader.ReadBytes(3);
                            var fileName = reader.ReadBytes(reader.ReadByte()); //文件名称
                            reader.ReadByte();
                            reader.ReadBytes(reader.ReadByte()); //文件大小
                            result.Snippets.Add(new TextSnippet(Encoding.UTF8.GetString(fileName),
                                MessageType.OfflineFile));
                            break;
                        }
                        case 0x19: //红包秘钥段
                        {
                            if (reader.ReadByte() != 0xC2)
                            {
                                break;
                            }
                            reader.ReadBytes(19);
                            reader.ReadBytes(reader.ReadByte()); //恭喜发财
                            reader.ReadByte();
                            reader.ReadBytes(reader.ReadByte()); //赶紧点击拆开吧
                            reader.ReadByte();
                            reader.ReadBytes(reader.ReadByte()); //QQ红包
                            reader.ReadBytes(5);
                            reader.ReadBytes(reader.ReadByte()); //[QQ红包]恭喜发财
                            reader.ReadBytes(22);
                            var redId = Encoding.UTF8.GetString(reader.ReadBytes(32)); //redid
                            reader.ReadBytes(12);
                            reader.ReadBytes(reader.BeReadChar());
                            reader.ReadBytes(0x10);
                            var key1 = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadByte())); //Key1
                            reader.BeReadChar();
                            var key2 = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadByte())); //Key2
                            result.Snippets.Add(new TextSnippet("", MessageType.RedBag, ("RedId", redId),
                                ("Key1", key1), ("Key2", key2)));
                            break;
                        }
                    }

                    reader.ReadBytes((int) (pos + dataLength - reader.BaseStream.Position));
                    messageType = reader.ReadByte();
                    dataLength = reader.BeReadChar();
                    pos = reader.BaseStream.Position;
                }
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public static Richtext FromLiteral(string message)
        {
            return new Richtext { Snippets = new List<TextSnippet> { new TextSnippet(message ?? "") } };
        }

        public static Richtext FromSnippets(params TextSnippet[] message)
        {
            return new Richtext { Snippets = message.ToList() };
        }

        public override string ToString()
        {
            return string.Join("", Snippets);
        }

        public static implicit operator string(Richtext text)
        {
            return text?.ToString();
        }

        public static implicit operator Richtext(string text)
        {
            return FromLiteral(text);
        }

        public static implicit operator Richtext(TextSnippet text)
        {
            return FromSnippets(text);
        }
    }

    public class TextSnippet
    {
        public string Content;
        public MessageType Type;
        private Dictionary<string, object> _data;

        public T Get<T>(string name, T value = default(T))
        {
            return (T) _data[name];
        }

        public void Set<T>(string name, T value)
        {
            _data[name] = value;
        }

        public TextSnippet(string message = "", MessageType type = MessageType.Normal,
            params (string name, object value)[] data)
        {
            Content = message;
            Type = type;
            foreach (var valueTuple in data)
            {
                Set(valueTuple.name, valueTuple.value);
            }
        }

        public int Length
        {
            get
            {
                switch (Type)
                {
                    case MessageType.Normal:
                        return Encoding.UTF8.GetByteCount(Content);
                    case MessageType.Emoji:
                        return 12;
                    case MessageType.Picture:
                    case MessageType.Xml:
                    case MessageType.Json:
                    case MessageType.At:
                    case MessageType.Shake:
                    case MessageType.ExitGroup:
                    case MessageType.GetGroupImformation:
                    case MessageType.AddGroup:
                        return 0;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override string ToString()
        {
            switch (Type)
            {
                case MessageType.Normal:
                    return Content;
                case MessageType.Shake:
                    return "[窗口抖动]";
                case MessageType.Picture:
                    return $"[图片{Content}]";
                case MessageType.Xml:
                    return $"[XML代码{Content}]";
                case MessageType.Json:
                    return $"[JSON代码{Content}]";
                case MessageType.Emoji:
                    return $"[表情{Content}]";
                case MessageType.At:
                    return $"[{Content}]";
                case MessageType.Audio:
                    return $"[音频{Content}]";
                case MessageType.Video:
                    return $"[视频{Content}]";
                case MessageType.ExitGroup:
                    return "[退出群]";
                case MessageType.GetGroupImformation:
                    return "[获取群信息]";
                case MessageType.AddGroup:
                    return "[加群]";
                case MessageType.OfflineFile:
                    return $"[离线文件{Content}]";
                default:
                    return "[特殊代码]";
            }
        }

        public static implicit operator string(TextSnippet text)
        {
            return text?.ToString();
        }

        public static implicit operator TextSnippet(string text)
        {
            return new TextSnippet(text);
        }
    }
}