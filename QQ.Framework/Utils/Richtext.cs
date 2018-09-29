using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QQ.Framework.Utils
{
    public class Richtext
    {
        public List<TextSnippet> Snippets = new List<TextSnippet>();

        public static Richtext Parse(string message)
        {
            // TODO: 解析富文本
            return FromLiteral(message);
        }

        public static Richtext FromLiteral(string message)
        {
            return new Richtext {Snippets = new List<TextSnippet> {new TextSnippet(message ?? "")}};
        }

        public static Richtext FromSnippets(params TextSnippet[] message)
        {
            return new Richtext {Snippets = message.ToList()};
        }

        public override string ToString()
        {
            return string.Join("", Snippets);
        }
    }

    public class TextSnippet
    {
        public string Content;
        public MessageType Type;

        public TextSnippet(string message = "", MessageType type = MessageType.Normal)
        {
            Content = message;
            Type = type;
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
                default:
                    return "[特殊代码]";
            }
        }
    }
}