using System.Collections.Generic;

namespace QQ.Framework.Utils
{
    public class Richtext
    {
        public List<TextSnippet> Snippets = new List<TextSnippet>();

        public static Richtext FromLiteral(string message)
        {
            return new Richtext
            {
                Snippets = new List<TextSnippet>
                {
                    new TextSnippet {Content = message}
                }
            };
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