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
                while (reader.BaseStream.Position + dataLength < reader.BaseStream.Length)
                {
                    reader.ReadByte();
                    var messageData = reader.ReadBytes(reader.BeReadChar());
                    switch (messageType)
                    {
                        case 0x01: //文本消息
                        {
                            var messageStr = Encoding.UTF8.GetString(messageData);
                            if (messageStr.Contains("@"))
                            {
                                //Reader.ReadBytes(10);
                                //var AtQQ = Util.GetQQNumRetUint(Util.ToHex(Reader.ReadBytes(4)));//被At人的QQ号
                                result.Snippets.Add(new TextSnippet
                                {
                                    Content = messageStr,
                                    Type = MessageType.At
                                });
                            }
                            else
                            {
                                result.Snippets.Add(new TextSnippet
                                {
                                    Content = messageStr,
                                    Type = MessageType.Normal
                                });
                            }

                            break;
                        }

                        case 0x02: //小黄豆表情
                        {
                            result.Snippets.Add(new TextSnippet
                            {
                                Content = Util.GetQQNumRetUint(Util.ToHex(messageData)).ToString(),
                                Type = MessageType.Emoji
                            });
                            break;
                        }
                        case 0x03: //图片
                        {
                            result.Snippets.Add(new TextSnippet
                            {
                                Content = Encoding.UTF8.GetString(messageData),
                                Type = MessageType.Picture
                            });
                            break;
                        }
                        case 0x0A: //音频
                        {
                            result.Snippets.Add(new TextSnippet
                            {
                                Content = Encoding.UTF8.GetString(messageData),
                                Type = MessageType.Audio
                            });
                            break;
                        }
                        case 0x0E: //未知
                        {
                            break;
                        }
                        case 0x19: //红包秘钥段
                        {
                            var redBagReader = new BinaryReader(new MemoryStream(messageData));
                            redBagReader.ReadBytes(20);
                            redBagReader.ReadBytes(redBagReader.ReadByte()); //恭喜发财
                            redBagReader.ReadByte();
                            redBagReader.ReadBytes(redBagReader.ReadByte()); //赶紧点击拆开吧
                            redBagReader.ReadByte();
                            redBagReader.ReadBytes(redBagReader.ReadByte()); //QQ红包
                            redBagReader.ReadBytes(5);
                            redBagReader.ReadBytes(redBagReader.ReadByte()); //[QQ红包]恭喜发财
                            redBagReader.ReadBytes(22);
                            var redId = Encoding.UTF8.GetString(redBagReader.ReadBytes(32)); //redid
                            redBagReader.ReadBytes(12);
                            redBagReader.ReadBytes(redBagReader.BeReadChar());
                            redBagReader.ReadBytes(0x10);
                            var key1 = Encoding.UTF8.GetString(redBagReader.ReadBytes(redBagReader.ReadByte())); //Key1
                            redBagReader.BeReadChar();
                            var key2 = Encoding.UTF8.GetString(redBagReader.ReadBytes(redBagReader.ReadByte())); //Key2
                            result.Snippets.Add(new TextSnippet("", MessageType.RedBag, ("RedId", redId),
                                ("Key1", key1), ("Key2", key2)));
                            break;
                        }
                    }
                    messageType = reader.ReadByte();
                    dataLength = reader.BeReadChar();
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
            return (T)_data[name];
        }

        public void Set<T>(string name, T value)
        {
            _data[name] = value;
        }

        public TextSnippet(string message = "", MessageType type = MessageType.Normal, params (string name, object value)[] data)
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
                    return $"[@{Content}]";
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