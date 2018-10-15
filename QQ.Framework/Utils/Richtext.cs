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
                var dataLength = reader.BeReadUInt16();
                var pos = reader.BaseStream.Position;
                while (pos + dataLength < reader.BaseStream.Length)
                {
                    reader.ReadByte();
                    switch (messageType)
                    {
                        case 0x01: // 纯文本消息、@
                        {
                            var messageStr = reader.BeReadString();
                            if (messageStr.StartsWith("@") && pos + dataLength - reader.BaseStream.Position == 16)
                            {
                                reader.ReadBytes(10);
                                result.Snippets.Add(new TextSnippet(messageStr, MessageType.At,
                                    ("Target", reader.BeReadUInt32())));
                            }
                            else
                            {
                                result.Snippets.Add(messageStr);
                            }

                            break;
                        }
                        case 0x02: // Emoji(系统表情)
                        {
                            reader.BeReadUInt16(); // 这里的数字貌似总是1：系统表情只有208个。
                            result.Snippets.Add(new TextSnippet("", MessageType.Emoji, ("Type", reader.ReadByte())));
                            break;
                        }
                        case 0x03: // 图片
                        {
                            result.Snippets.Add(new TextSnippet(reader.BeReadString(), MessageType.Picture));
                            break;
                        }
                        case 0x0A: // 音频
                        {
                            result.Snippets.Add(new TextSnippet(reader.BeReadString(), MessageType.Audio));
                            break;
                        }
                        case 0x0E: // 未知
                        {
                            break;
                        }
                        case 0x12: // 群名片
                        {
                            break;
                        }
                        case 0x14: // XML
                        {
                            reader.ReadByte();
                            result.Snippets.Add(new TextSnippet(
                                GZipByteArray.DecompressString(reader.ReadBytes((int) (reader.BaseStream.Length - 1))),
                                MessageType.Xml));
                            break;
                        }
                        case 0x18: // 群文件
                        {
                            reader.ReadBytes(5);
                            var fileName = reader.BeReadString(); // 文件名称... 长度总是一个byte
                            reader.ReadByte();
                            reader.ReadBytes(reader.ReadByte()); // 文件大小
                            result.Snippets.Add(new TextSnippet(fileName, MessageType.OfflineFile));
                            break;
                        }
                        case 0x19: // 红包秘钥段
                        {
                            if (reader.ReadByte() != 0xC2)
                            {
                                break;
                            }

                            reader.ReadBytes(19);
                            reader.ReadBytes(reader.ReadByte()); // 恭喜发财
                            reader.ReadByte();
                            reader.ReadBytes(reader.ReadByte()); // 赶紧点击拆开吧
                            reader.ReadByte();
                            reader.ReadBytes(reader.ReadByte()); // QQ红包
                            reader.ReadBytes(5);
                            reader.ReadBytes(reader.ReadByte()); // [QQ红包]恭喜发财
                            reader.ReadBytes(22);
                            var redId = Encoding.UTF8.GetString(reader.ReadBytes(32)); //redid
                            reader.ReadBytes(12);
                            reader.ReadBytes(reader.BeReadUInt16());
                            reader.ReadBytes(0x10);
                            var key1 = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadByte())); //Key1
                            reader.BeReadUInt16();
                            var key2 = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadByte())); //Key2
                            result.Snippets.Add(new TextSnippet("", MessageType.RedBag, ("RedId", redId),
                                ("Key1", key1), ("Key2", key2)));
                            break;
                        }
                    }

                    reader.ReadBytes((int) (pos + dataLength - reader.BaseStream.Position));
                    messageType = reader.ReadByte();
                    dataLength = reader.BeReadUInt16();
                    pos = reader.BaseStream.Position;
                }
            }
            catch (Exception ex)
            {
            }

            // 移除所有空白的片段
            result.Snippets.RemoveAll(s => s.Type == MessageType.Normal && string.IsNullOrEmpty(s.Content));

            // 若长度大于1，那么应该只含有普通文本、At、表情、图片。
            // 虽然我看着别人好像视频也能通过转发什么的弄进来，但是反正我们现在不支持接收音视频，所以不管了
            return result.Snippets.Count > 1 && result.Snippets.Any(s =>
                       s.Type != MessageType.Normal && s.Type != MessageType.At &&
                       s.Type != MessageType.Emoji && s.Type != MessageType.Picture)
                ? throw new NotSupportedException("富文本中包含多个非聊天代码")
                : result;
        }

        public static Richtext FromLiteral(string message)
        {
            return new Richtext
            {
                Snippets = new List<TextSnippet>
                {
                    new TextSnippet(message ?? "")
                }
            };
        }

        public static Richtext FromSnippets(params TextSnippet[] message)
        {
            return new Richtext
            {
                Snippets = message.ToList()
            };
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
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        public object this[string key]
        {
            get => Get<object>(key);
            set => Set(key, value);
        }

        public T Get<T>(string name, T value = default(T))
        {
            if (_data.ContainsKey(name))
            {
                return (T) _data[name];
            }

            return value;
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
                    case MessageType.RedBag:
                    case MessageType.Audio:
                    case MessageType.Video:
                    case MessageType.OfflineFile:
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
                    return $"[表情{this["Type"]}]";
                case MessageType.At:
                    return $"[{this["Target"]}]";
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