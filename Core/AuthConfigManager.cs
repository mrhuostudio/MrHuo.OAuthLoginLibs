using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using MrHuo.OAuthLoginLibs.Resources;

namespace MrHuo.OAuthLoginLibs.Core
{
    /// <summary>
    /// OAuth配置管理器
    /// </summary>
    public class AuthConfigManager
    {
        /// <summary>
        /// 获取目录下所有配置的OAuth配置
        /// 目录为：如果未在.config文件AppSettings节定义OAuthLoginConfigPath，那么目录默认为~/Configs。
        /// <para>注意：目录配置请使用相对目录</para>
        /// </summary>
        /// <returns></returns>
        public static List<OAuthLoginConfig> GetAllLoginConfigs()
        {
            Check.IsWebEnvironment();

            List<OAuthLoginConfig> localConfigs = new List<OAuthLoginConfig>();
            var configPath = ConfigurationManager.AppSettings[RS.get(ResourceKey.SETTINGS_OAuthLoginConfigPath)];
            if (configPath == null)
            {
                configPath = "~/Configs";
            }
            configPath = HttpContext.Current.Server.MapPath(configPath);

            if (!Directory.Exists(configPath))
            {
                throw Errors.OAuthLoginConfigDirectoryNotExist(configPath);
            }
            var configs = Directory.GetFiles(configPath, "*OAuthConfig.config", SearchOption.TopDirectoryOnly);
            foreach (var file in configs)
            {
                try
                {
                    var c = OAuthLoginConfig.Parse(file);
                    localConfigs.Add(c);
                }
                catch (Exception ex)
                {
                    Log.Debug(typeof(AuthConfigManager), ex);
                }
            }

            return localConfigs;
        }

        /// <summary>
        /// 根据平台获取相应的配置类
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        public static OAuthLoginConfig GetConfigByPlatform(string platform)
        {
            var config = GetAllLoginConfigs().SingleOrDefault(p => p.Platform.IsFullEqual(platform));
            return config;
        }
    }
}