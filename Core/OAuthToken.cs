using System;
using Codeplex.Data;

namespace MrHuo.OAuthLoginLibs.Core
{
    /// <summary>
    /// 可序列化的OAuthToken基类。
    /// <para>用于登陆之后的后续开发。</para>
    /// <para>一般平台用这个就够了</para>
    /// </summary>
    [Serializable]
    public class OAuthToken
    {
        /// <summary>
        /// AccessToken
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// AccessToken过期时间
        /// </summary>
        public int ExpiresAt { get; set; }

        /// <summary>
        /// 获取与实际账号对应OpenId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 重写ToString()方法。返回此类的Json格式字符串。
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