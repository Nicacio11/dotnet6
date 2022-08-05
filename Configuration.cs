using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog;
public static class Configuration
{
    public static string JwtTokenKey 
    { 
        get
        {
            return "YWUzNjY3ZjYtNDA4NC00MDJjLTkzODItZTE1ZDBhZjk0MDZk";
        } 
    }

    public static string ApiKeyName = "api_key";
    public static string ApiKey = "api_key/YWUzNjY3ZjYtNDA4NC00MDJjLTkzODItZTE1ZDBhZjk0MDZk/YWUzNjY3ZjYtNDA4NC00MDJ";

}