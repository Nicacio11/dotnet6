using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Configurations
{
    public static class ConfigurationsSettings
    {
        public static void AddConfiguration(this WebApplicationBuilder builder)
        {
            Configuration.JwtTokenKey = builder.Configuration.GetValue<string>("JwtTokenKey");
            Configuration.ApiKeyName = builder.Configuration.GetValue<string>("ApiKeyName");
            Configuration.ApiKey = builder.Configuration.GetValue<string>("ApiKey");
        
            var smpt = new Configuration.SmtpConfiguration();
            builder.Configuration.GetSection("Smtp").Bind(smpt);
            Configuration.Smtp = smpt;
        }
        
    }
}