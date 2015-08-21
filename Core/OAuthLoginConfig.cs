using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Codeplex.Data;

namespace MrHuo.OAuthLoginLibs.Core
{
    /// <summary>
    /// 可序列化的OAuth配置类
    /// </summary>
    [Serializable]
    public class OAuthLoginConfig
    {
        /// <summary>
        /// 仅适用于输出配置文件时
        /// </summary>
        public int DisplayIndex { get; set; }

        /// <summary>
        /// 当前配置文件是否起作用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 配置文件所属平台
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// AppKey
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// AppSecret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// Api基地址
        /// </summary>
        public string ApiBaseUrl { get; set; }

        /// <summary>
        /// 登录成功回调地址
        /// </summary>
        public string CallBackUrl { get; set; }

        /// <summary>
        /// AuthorizeUrl地址模板
        /// </summary>
        public string AuthorizeUrlTemplate { get; set; }

        /// <summary>
        /// ApiToken获取地址
        /// </summary>
        public string ApiTokenUrl { get; set; }

        /// <summary>
        /// ApiToken的获取方式
        /// </summary>
        public string ApiTokenGetMothed { get; set; }

        /// <summary>
        /// 获取ApiToken所需的参数配置
        /// </summary>
        public Dictionary<string, string> ApiTokenParams { get; set; }

        /// <summary>
        /// 回调地址是否URL编码
        /// </summary>
        public bool UrlEncode { get; set; }

        /// <summary>
        /// 将一个文件名为xmlFileName的配置文件转化到配置类
        /// </summary>
        /// <param name="xmlFileName"></param>
        /// <returns></returns>
        public static OAuthLoginConfig Parse(string xmlFileName)
        {
            var ret = new OAuthLoginConfig();
            if (!File.Exists(xmlFileName))
            {
                throw Errors.OAuthLoginConfigFileNotExist(xmlFileName);
            }

            var doc = XDocument.Load(xmlFileName);

            var displayIndex = doc.Descendants("DisplayIndex").Single();
            ret.DisplayIndex = int.Parse(displayIndex.Value);

            var enabled = doc.Descendants("Enabled").Single();
            ret.Enabled = bool.Parse(enabled.Value);

            var encord = doc.Descendants("UrlEncode").Single();
            ret.UrlEncode = bool.Parse(encord.Value);

            var platform = doc.Descendants("Platform").Single();
            ret.Platform = platform.Value;

            var appkey = doc.Descendants("AppKey").Single();
            ret.AppKey = appkey.Value;

            var appsecret = doc.Descendants("AppSecret").Single();
            ret.AppSecret = appsecret.Value;

            var baseurl = doc.Descendants("ApiBaseUrl").Single();
            ret.ApiBaseUrl = baseurl.Value;

            var callback = doc.Descendants("CallBackUrl").Single();
            ret.CallBackUrl = callback.Value;

            var authUrl = doc.Descendants("AuthorizeUrlTemplate").Single();
            ret.AuthorizeUrlTemplate = authUrl.Value;

            var tokenUrl = doc.Descendants("ApiTokenUrl").Single();
            ret.ApiTokenUrl = tokenUrl.Value;

            var tokenGetMethod = doc.Descendants("ApiTokenGetMothed").Single();
            ret.ApiTokenGetMothed = tokenGetMethod.Value;

            var list = doc.Descendants("ApiTokenParams").Elements();
            ret.ApiTokenParams = new Dictionary<string, string>();

            foreach (var item in list)
            {
                ret.ApiTokenParams.Add(item.Name.LocalName, item.Value);
            }
            return ret;
        }

        /// <summary>
        /// 重写ToString方法。用于返回此类的Json格式字符串。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            try
            {
                return DynamicJson.Serialize(this);
            }
            catch (Exception ex)
            {
                return base.ToString();
            }
        }
    }
}