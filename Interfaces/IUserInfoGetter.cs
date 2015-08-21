namespace MrHuo.OAuthLoginLibs.Interfaces
{
    /// <summary>
    /// 通用的用于扩展的获取用户信息的接口
    /// </summary>
    public interface IUserInfoGetter<TUserInfo>
    {
        /// <summary>
        /// 获取用户接口
        /// </summary>
        /// <returns></returns>
        TUserInfo GetUserInfo();
    }
}