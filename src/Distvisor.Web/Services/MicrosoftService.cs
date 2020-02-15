using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IMicrosoftService
    {
        string GetAuthorizeUrl();
    }

    public class MicrosoftService : IMicrosoftService
    {
        public string GetAuthorizeUrl()
        {
            return $"https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize" +
                "?client_id=" +
                "&response_type=code" +
                "&redirect_uri=" +
                "&response_mode = query" +
                "&scope=" +
                "&state=";
        }
    }
}
