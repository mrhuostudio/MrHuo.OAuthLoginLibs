using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Codeplex.Data;

namespace System
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 将一个Url地址编码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ToEncodeUrl(this string url)
        {
            return HttpUtility.UrlEncode(url);
        }

        /// <summary>
        /// 根据查询字符串获取Dynamic对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static dynamic QueryStringToObject(this string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return null;
            }

            StringBuilder json = new StringBuilder();
            json.Append("{");
            var index = 0;
            foreach (var item in query.Split('&'))
            {
                var key = item.Split('=')[0];
                var value = item.Split('=')[1];
                json.Append("\"" + key + "\":\"" + value + "\"");
                if (++index < query.Split('&').Length)
                {
                    json.Append(",");
                }
            }
            json.Append("}");

            var jsonObject = DynamicJson.Parse(json.ToString());
            return jsonObject;
        }

        /// <summary>
        /// 根据Json字符串，获取Dynamic对象
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static dynamic StringToObject(this string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return null;
            }
            Regex regQueryString = new Regex("[&](.+?)=");
            if (regQueryString.IsMatch(content))
            {
                return content.QueryStringToObject();
            }
            if ((content.StartsWith("{") && content.EndsWith("}")) || (content.StartsWith("[") && content.EndsWith("]")))
            {
                return DynamicJson.Parse(content);
            }
            return null;
        }

        /// <summary>
        /// 判断两个字符串是否忽略大小写完全相等
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool IsFullEqual(this string src, string dest)
        {
            return src.Equals(dest, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}