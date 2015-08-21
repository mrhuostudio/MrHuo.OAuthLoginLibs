using System;
using System.Net;
using System.Text;
using System.Web;
using MrHuo.OAuthLoginLibs.Exceptions;
using MrHuo.OAuthLoginLibs.Interfaces;
using RestSharp;

namespace MrHuo.OAuthLoginLibs.Core
{
    /// <summary>
    /// OAuth登录核心操作类
    /// </summary>
    public class OAuthLogin : IOAuthLogin,IDisposable
    {
        public OAuthLogin()
        {
        }

        private static OAuthLogin _instance = null;

        /// <summary>
        /// 获取当前OAuth的实例
        /// </summary>
        public static OAuthLogin Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OAuthLogin();
                }
                return _instance;
            }
        }

        #region [Privates]

        /// <summary>
        /// 创建Token获取的RestRequest
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        private RestRequest CreateTokenRequest(string code, string state, OAuthLoginConfig config)
        {
            RestRequest request = null;
            if ("get".IsFullEqual(config.ApiTokenGetMothed))
            {
                StringBuilder queryString = new StringBuilder();
                int index = 0;
                foreach (var p in config.ApiTokenParams)
                {
                    var value = p.Value.Replace("{AppKey}", config.AppKey)
                                .Replace("{AppSecret}", config.AppSecret)
                                .Replace("[state]", state)
                                .Replace("[code]", code);

                    if (config.UrlEncode)
                    {
                        value = value.Replace("{CallBackUrl}", config.CallBackUrl.ToEncodeUrl());
                    }
                    else
                    {
                        value = value.Replace("{CallBackUrl}", config.CallBackUrl);
                    }
                    queryString.Append(p.Key + "=" + value);
                    if (++index < config.ApiTokenParams.Count)
                    {
                        queryString.Append("&");
                    }
                }
                var url = config.ApiTokenUrl + "?" + queryString.ToString();
                request = new RestRequest(url, Method.GET);
            }
            else if ("post".IsFullEqual(config.ApiTokenGetMothed))
            {
                request = new RestRequest(config.ApiTokenUrl, Method.POST);
                foreach (var p in config.ApiTokenParams)
                {
                    var value = p.Value.Replace("{AppKey}", config.AppKey)
                                .Replace("{AppSecret}", config.AppSecret)
                                .Replace("[state]", state)
                                .Replace("[code]", code);

                    if (config.UrlEncode)
                    {
                        value = value.Replace("{CallBackUrl}", config.CallBackUrl.ToEncodeUrl());
                    }
                    else
                    {
                        value = value.Replace("{CallBackUrl}", config.CallBackUrl);
                    }
                    request.AddParameter(p.Key, value, ParameterType.GetOrPost);
                }
            }
            return request;
        }

        /// <summary>
        /// 根据配置文件获取AuthorationUrl
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="scope"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        private string CreateAuthorationUrlByConfig(string platform, string scope, OAuthLoginConfig config)
        {
            string state = AuthStateManager.RequestState(HttpContext.Current.Session.SessionID, platform);
            string authUrl = config.AuthorizeUrlTemplate
                             .Replace("{AppKey}", config.AppKey)
                             .Replace("{AppSecret}", config.AppSecret)
                             .Replace("{ApiBaseUrl}", config.ApiBaseUrl)
                             .Replace("[state]", state)
                             .Replace("[time]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                             .Replace("{scope}", scope);

            if (config.UrlEncode)
            {
                authUrl = authUrl.Replace("{CallBackUrl}", config.CallBackUrl.ToEncodeUrl());
            }
            else
            {
                authUrl = authUrl.Replace("{CallBackUrl}", config.CallBackUrl);
            }
            return authUrl;
        }

        #endregion [Privates]

        #region [Protecteds]

        /// <summary>
        /// 执行网络请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected internal virtual IRestResponse Execute(string url, RestRequest request)
        {
            RestClient _restClient = new RestClient(url);
            var response = _restClient.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw Errors.ServerExecuteException(response);
            }
            return response;
        }

        #endregion [Protecteds]

        #region IOAuthLogin 成员

        /// <summary>
        /// 获取Authoration URL并跳转
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="scope"></param>
        public virtual void BeginAuthoration(string platform, string scope = "")
        {
            var config = AuthConfigManager.GetConfigByPlatform(platform);
            if (config == null)
            {
                var err = Errors.NotSupportedPlatform(platform);
                Log.Debug(typeof(OAuthLogin), err);

                throw err;
            }
            if (config.Enabled)
            {
                var authUrl = CreateAuthorationUrlByConfig(platform, scope, config);
                HttpContext.Current.Response.Redirect(authUrl, true);
            }
            else
            {
                var err = Errors.OAuthLoginNotEnabled(platform);
                Log.Debug(typeof(OAuthLogin), err);

                throw err;
            }
        }

        /// <summary>
        /// 执行OAuth验证登陆
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pageState"></param>
        /// <param name="loginCallback"></param>
        /// <param name="errorCallback"></param>
        public virtual OAuthLoginResult Login(string code, string pageState)
        {
            var oauthLoginResult = new OAuthLoginResult();
            try
            {
                var platform = AuthStateManager.GetPlatformByState(pageState);
                var config = AuthConfigManager.GetConfigByPlatform(platform);
                if (config == null)
                {
                    oauthLoginResult.Error = Errors.NotConfigForPlatform(platform);
                    return oauthLoginResult;
                }
                if (config.Enabled)
                {
                    AuthStateManager.RemoveState(HttpContext.Current.Session.SessionID, pageState);

                    RestRequest request = CreateTokenRequest(code, pageState, config);
                    var content = Execute(config.ApiBaseUrl, request).Content;

                    oauthLoginResult.Config = config;
                    oauthLoginResult.ServerResponse = content;

                    Log.Debug(typeof(OAuthLogin), content);
                    return oauthLoginResult;
                }

                oauthLoginResult.Error = Errors.OAuthLoginNotEnabled(platform);
            }
            catch (Exception ex)
            {
                if (ex is OAuthException)
                {
                    Log.Debug(typeof(OAuthLogin), "Server Response：" + (ex as OAuthException).Response.Content);
                }
                Log.Debug(typeof(OAuthLogin), ex);
                oauthLoginResult.Error = ex;
            }
            return oauthLoginResult;
        }

        #endregion IOAuthLogin 成员

        #region IDisposable 成员
        protected void Dispose(bool isDisposable)
        {
            if (isDisposable)
            {
                AuthStateManager.Clear();

                if (_instance != null)
                {
                    GC.SuppressFinalize(_instance);
                    GC.ReRegisterForFinalize(_instance);
                }
                GC.SuppressFinalize(this);
                GC.ReRegisterForFinalize(this);
            }
            GC.Collect();
        }
        /// <summary>
        /// 释放系统资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable 成员
    }
}