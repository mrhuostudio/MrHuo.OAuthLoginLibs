using System;
using MrHuo.OAuthLoginLibs.Interfaces;
using RestSharp;

namespace MrHuo.OAuthLoginLibs.Core
{
    /// <summary>
    /// OAuthContextBase基类
    /// </summary>
    public abstract class OAuthContextBase<TOAuthToken, TUserInfo> :
        IOpenIdGetter<TOAuthToken>,
        IUserInfoGetter<TUserInfo>
        where TOAuthToken : OAuthToken
    {
        protected OAuthLoginConfig _config;
        protected string _accessCallbackString;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="config"></param>
        /// <param name="accessTokenCallbackString"></param>
        public OAuthContextBase(OAuthLoginConfig config, string accessTokenCallbackString)
        {
            this._config = config;
            this._accessCallbackString = accessTokenCallbackString;
        }

        /// <summary>
        /// 提供给子类的调试入口
        /// </summary>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        protected virtual void Debug(Type type,  string msg)
        {
            Log.Debug(type, msg);
        }

        /// <summary>
        /// 请求服务器数据的方法
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected string Request(IRestRequest request)
        {
            return Request(this._config.ApiBaseUrl, request);
        }

        /// <summary>
        /// 用指定的资源基地址，访问服务器数据
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected string Request(string baseUrl, IRestRequest request)
        {
            RestClient restClent = new RestClient(baseUrl);
            var response = restClent.Execute(request).Content;
            return response;
        }

        #region IUserInfoGetter 成员

        /// <summary>
        /// 未实现的IUserInfoGetter接口成员，重写用于获取用户信息
        /// </summary>
        /// <returns></returns>
        public virtual TUserInfo GetUserInfo()
        {
            throw new NotImplementedException();
        }

        #endregion IUserInfoGetter 成员

        #region IOpenIdGetter<TOAuthToken> 成员

        /// <summary>
        /// 必须重写的IOpenIdGetter成员
        /// </summary>
        /// <returns></returns>
        public abstract TOAuthToken GetOAuthToken();

        #endregion IOpenIdGetter<TOAuthToken> 成员
    }
}