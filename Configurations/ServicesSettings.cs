using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Services;
using Microsoft.EntityFrameworkCore;

namespace Blog.Configurations
{
    public static class ServicesSettings
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BlogDataContext>( x => x.UseSqlServer(connectionString));
            
            // sempre vai criar uma instancia nova quando chamar um método
            services.AddTransient<TokenService>();
            services.AddTransient<EmailService>();
            // builder.Services.AddScoped<TokenService>(); // Vai durar por transação, dentro de um escopo de método
            // builder.Services.AddSingleton<TokenService>(); // vai vai durar para sempre uma instancia para toda aplicação
        }
    }
}