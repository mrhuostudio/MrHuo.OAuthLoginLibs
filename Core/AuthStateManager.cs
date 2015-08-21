using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace MrHuo.OAuthLoginLibs.Core
{
    /// <summary>
    /// Auth状态管理器
    /// </summary>
    internal class AuthStateManager
    {
        /// <summary>
        /// 内部用于保存状态的列表
        /// </summary>
        private static List<AuthState> oauthLoginStates = new List<AuthState>();

        private static object lockObj = new object();

        /// <summary>
        /// 在请求获取Authoration Url的时候请求获取此次操作状态
        /// </summary>
        /// <param name="sessionId">当前回话Id</param>
        /// <param name="platform">平台</param>
        /// <returns>AuthState</returns>
        public static string RequestState(string sessionId, string platform)
        {
            //加入线程锁，保证每次获取状态都正确
            Monitor.Enter(lockObj);
            try
            {
                //如果同一sessionId和登录平台有未处理完成的状态，就移除
                var exists = oauthLoginStates.Where((p) => p.sessionId.IsFullEqual(sessionId) && p.platform.IsFullEqual(platform)).ToArray();

                if (exists.Count() > 0)
                {
                    for (int i = 0; i < exists.Count(); i++)
                    {
                        oauthLoginStates.Remove(exists[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug(typeof(AuthStateManager), ex);
            }

            //生成新的状态，此状态会在登录完成后移除
            var state = Guid.NewGuid().ToString();
            oauthLoginStates.Add(new AuthState(sessionId, platform, state));
            var sessionKey = platform + "_" + sessionId + "_OAuthState";
            HttpContext.Current.Session.Add(sessionKey, state);

            Monitor.Exit(lockObj);
            return state;
        }

        /// <summary>
        /// 在登录操作完毕之后会移除此状态
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="state"></param>
        public static void RemoveState(string sessionId, string state)
        {
            var st = oauthLoginStates.SingleOrDefault(p => p.state.IsFullEqual(state) && p.sessionId.IsFullEqual(sessionId));
            if (st == null)
            {
                throw Errors.NoCSRF();
            }
            var sessionKey = st.platform + "_" + sessionId + "_OAuthState";
            oauthLoginStates.Remove(st);
            HttpContext.Current.Session.Remove(sessionKey);
        }

        /// <summary>
        /// 根据服务器传回来的state获取此次操作的平台
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string GetPlatformByState(string state)
        {
            var ret = oauthLoginStates.SingleOrDefault(p => p.state.IsFullEqual(state));
            if (ret != null)
            {
                return ret.platform;
            }
            throw Errors.NoCSRF();
        }

        /// <summary>
        /// 清空状态集合，用于OAuth类销毁之后清理资源
        /// </summary>
        public static void Clear()
        {
            oauthLoginStates.Clear();
        }
    }

    /// <summary>
    /// 内部类，表示AuthState
    /// </summary>
    internal class AuthState
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="session"></param>
        /// <param name="plat"></param>
        /// <param name="state"></param>
        public AuthState(string session, string plat, string state)
        {
            this.sessionId = session;
            this.platform = plat;
            this.state = state;
        }

        /// <summary>
        /// 会话ID。用于多个用户同时操作时加以区分
        /// </summary>
        public string sessionId { get; set; }

        /// <summary>
        /// 操作平台
        /// </summary>
        public string platform { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string state { get; set; }
    }
}