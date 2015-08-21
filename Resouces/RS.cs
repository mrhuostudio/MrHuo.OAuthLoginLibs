using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace MrHuo.OAuthLoginLibs.Resources
{
    internal static class RS
    {
        private static ResourceManager rm;

        private static CultureInfo getCurrentCulture()
        {
            return Thread.CurrentThread.CurrentCulture;
        }

        private static ResourceManager getResourceManager()
        {
            if (rm == null)
            {
                rm = new ResourceManager("MrHuo.OAuthLoginLibs.Resources.Resource", typeof(MrHuo.OAuthLoginLibs.Resources.RS).Assembly);
            }
            return rm;
        }

        internal static string get(string key)
        {
            try
            {
                return getResourceManager().GetString(key, getCurrentCulture());
            }
            catch (Exception ex)
            {
                return key;
            }
        }

        internal static string get(string key, string format)
        {
            var str = get(key);
            return string.Format(str, format);
        }
    }

    internal static class ResourceKey
    {
        //Settings Resource Key
        public static string SETTINGS_RecordOAuthLog = "SETTINGS_RecordOAuthLog";

        public static string SETTINGS_OAuthLoginConfigPath = "SETTINGS_OAuthLoginConfigPath";

        //Errors Resource Key
        public static string ERRORS_NotConfigForPlatform = "ERRORS_NotConfigForPlatform";

        public static string ERRORS_NotSupportedPlatform = "ERRORS_NotSupportedPlatform";
        public static string ERRORS_OnlySupportedWeb = "ERRORS_OnlySupportedWeb";
        public static string ERRORS_OAuthLoginNotEnabled = "ERRORS_OAuthLoginNotEnabled";
        public static string ERRORS_OAuthLoginConfigDirectoryNotExist = "ERRORS_OAuthLoginConfigDirectoryNotExist";
        public static string ERRORS_OAuthLoginConfigFileNotExist = "ERRORS_OAuthLoginConfigFileNotExist";
        public static string ERRORS_NoCSRF = "ERRORS_NoCSRF";
    }
}