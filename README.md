# MrHuo.OAuthLoginLibs
通用的可配置的OAuth2登录组件，只需增加配置文件就可支持新的第三方平台登录。
代码内容版权归mrhuo.com所有，修改必须通知作者。
邮箱：admin@mrhuo.com

简单介绍可见地址：http://www.cnblogs.com/MrHuo/p/new-way-to-do-oauthlogin.html

声明：
代码中使用开源库（版权归原作者所有）：
1、DynamicJson.dll  将json可转化为dynamic对象。
2、RestSharp        Simple REST and HTTP API Client

提供内容：
1、全部源代码。
2、最新DLL，可直接引用开发。
3、提供MrHuo.OAuthLogin.QQApis，为QQ平台用户信息获取相关API（仅包含用户信息获取，其他自行开发）。

使用方法：
1、（必须）引入DynamicJson.dll(有提供),RestSharp.dll(有提供),MrHuo.OAuthLoginLibs.dll
2、（必须）在网站根目录创建Configs目录，或在web.config/appsettings中创建名称为“OAuthLoginConfigPath”的项作为配置文件目录。作者已创建一些默认的配置文件，请替换配置中的AppKey，AppSecret等参数为自己的。
3、（可选）如果不需要记录日志，请在web.config/appsettings中创建名称为“RecordOAuthLog”的项，并将值设置为false。
4、（必须）创建入口文件。
用以下代码获取配置文件中的所有第三方登录(其中的代码可根据实际情况修改，此代码为MVC5 Razor代码)：
@{
  var platforms = AuthConfigManager.GetAllLoginConfigs().OrderBy(p => p.DisplayIndex);
  foreach (var config in platforms)
  {
    <input type="button" class="btn btn-default" value="@(config.Platform)登录" onclick="location.href='/Social/OAuth/@config.Platform'" @(!config.Enabled ? "disabled='disabled' title='未启用“" + config.Platform + "”登录'" : "") />
  }
}
5、（必须）后台开始验证时，可能需要以下代码（此处我写了Controller内的代码）：
    public class SocialController : Controller
    {
        OAuthLogin oauthLogin = new OAuthLogin();
        public ActionResult OAuth(string platform)
        {
            return getPlatformActionResult(platform);
        }
        public ActionResult LoginCallback(string code, string state)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                {
                    return View("Error", (object)("登录发生错误：" + Request.QueryString.ToString() + "<br />Url:" + Request.Url));
                }
                string ret = string.Empty;
                var result = oauthLogin.Login(code, state);
                if (result.Error != null)
                {
                    return View("Error", (object)result.Error.Message);
                }

                if ("QQ".IsFullEqual(result.Config.Platform))
                {
                    var qqContext = new QQContext(result.Config, result.ServerResponse);
                    var user = qqContext.GetUserInfo();
                    ret += user.NickName + ",<img src='" + user.Avatar + "' />," + user.Gender + "<br /><br />";
                }
                ret += "Platform " + result.Config.Platform + " Logined Result: <br /><br />" + result.ServerResponse;
                return View((object)ret);
            }
            catch (Exception ex)
            {
                return View("Error", (object)ex.Message);
            }
        }
        private ActionResult getPlatformActionResult(string platform)
        {
            try
            {
                oauthLogin.BeginAuthoration(platform);
            }
            catch (Exception ex)
            {
                return View("Error", (object)ex.Message);
            }
            return null;
        }
    }

注意：此段代码中以下代码：
if ("QQ".IsFullEqual(result.Config.Platform))
{
    var qqContext = new QQContext(result.Config, result.ServerResponse);
    var user = qqContext.GetUserInfo();
    ret += user.NickName + ",<img src='" + user.Avatar + "' />," + user.Gender + "<br /><br />";
}
为MrHuo.OAuthLogin.QQApis提供的功能。

6、当然，用户还需要从各个平台自行申请Appkey,AppSecret.

大致介绍如上。
详细咨询请加QQ：491217650（加QQ注明Github来源）.

有不足的地方，希望大家都齐心协力让这个组件走的更远。

团队网站：www.mrhuo.com
个人博客：mrhuo.cnblogs.com
邮件：admin@mrhuo.com
