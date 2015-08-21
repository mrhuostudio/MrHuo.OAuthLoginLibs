using System;
using System.IO;
using MrHuo.OAuthLoginLibs.Exceptions;
using MrHuo.OAuthLoginLibs.Resources;
using RestSharp;

namespace MrHuo.OAuthLoginLibs
{
    /// <summary>
    /// 内部整合的异常
    /// </summary>
    internal class Errors
    {
        public static Exception NotConfigForPlatform(string platform)
        {
            return new Exception(RS.get(ResourceKey.ERRORS_NotConfigForPlatform, platform));
        }

        public static Exception NotSupportedPlatform(string platform)
        {
            return new Exception(RS.get(ResourceKey.ERRORS_NotSupportedPlatform, platform));
        }

        public static Exception OnlySupportedWeb()
        {
            return new NotSupportedException(RS.get(ResourceKey.ERRORS_OnlySupportedWeb));
        }

        public static Exception OAuthLoginNotEnabled(string platform)
        {
            return new Exception(RS.get(ResourceKey.ERRORS_OAuthLoginNotEnabled, platform));
        }

        public static Exception OAuthLoginConfigDirectoryNotExist(string configPath)
        {
            return new DirectoryNotFoundException(RS.get(ResourceKey.ERRORS_OAuthLoginConfigDirectoryNotExist, configPath));
        }

        public static Exception OAuthLoginConfigFileNotExist(string configFile)
        {
            return new FileNotFoundException(RS.get(ResourceKey.ERRORS_OAuthLoginConfigFileNotExist), configFile);
        }

        public static OAuthException ServerExecuteException(IRestResponse response)
        {
            return new OAuthException(response);
        }

        public static Exception NoCSRF()
        {
            return new Exception(RS.get(ResourceKey.ERRORS_NoCSRF));
        }
    }
}