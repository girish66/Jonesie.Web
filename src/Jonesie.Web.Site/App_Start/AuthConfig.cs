using Jonesie.Web.Contracts.Core;
using Microsoft.Web.WebPages.OAuth;

namespace Jonesie.Web.Site
{
  public static class AuthConfig
  {
    public static void RegisterAuth(ISettings settings)
    {
      // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
      // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

      OAuthWebSecurity.RegisterMicrosoftClient(
          clientId: "000000004C0DF091",
          clientSecret: "NKedQPrnGiaTEFjild-Ts6SL-yiVffAy");

      //OAuthWebSecurity.RegisterTwitterClient(
      //    consumerKey: "",
      //    consumerSecret: "");

      //OAuthWebSecurity.RegisterFacebookClient(
      //    appId: "",
      //    appSecret: "");

      OAuthWebSecurity.RegisterGoogleClient();
    }
  }
}
