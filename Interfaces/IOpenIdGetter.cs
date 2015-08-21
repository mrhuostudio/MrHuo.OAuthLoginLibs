using MrHuo.OAuthLoginLibs.Core;

namespace MrHuo.OAuthLoginLibs.Interfaces
{
    /// <summary>
    /// 通用于其他平台的OpenId获取器
    /// </summary>
    public interface IOpenIdGetter<T> where T : OAuthToken
    {
        /// <summary>
        /// 根据accessTokenCallbackString获取OpenId，具体操作由开发者自行实现
        /// </summary>
        /// <returns></returns>
        T GetOAuthToken();
    }
}