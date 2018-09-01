using Microsoft.VisualStudio.TestTools.UnitTesting;
using QQ.Framework.Utils;

namespace QQ.FrameworkTest
{
    [TestClass]
    public class GZipByteArrayTest
    {
        [TestMethod]
        public void TestCompressBytes()
        {
            var input =
                "<?xml version='1.0' encoding='utf-8'?><msg templateID='12345' action='web' brief='芒果科技 的分享' serviceID='2' url='http://music.163.com/song/33668486/'>  <item layout='2'>    <audio src='http://m2.music.126.net/66NgS6mnDITOLBtojRlG2g==/3359008023015680.mp3' cover='http://www.qqmango.com/xz/baoshixit.png'/><title><![CDATA[[机器人昵称]为您报时]]></title><summary><![CDATA[[时间]]]></summary>  </item>  <item layout='0'><summary><![CDATA[[星期]－[农历]]]></summary></item>  <source action='web' name='报时系统' icon='http://www.qqmango.com/xz/baoshixit.png' url='http://www.baidu.com'/></msg>";
            var result = GZipByteArray.CompressBytes(input);
            var resultStr = Util.ToHex(result);
        }
    }
}