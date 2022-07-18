using Blog.Data;
using Blog.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => 
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddDbContext<BlogDataContext>();

// sempre vai criar uma instancia nova quando chamar um método
builder.Services.AddTransient<TokenService>();
// builder.Services.AddScoped<TokenService>(); // Vai durar por transação, dentro de um escopo de método
// builder.Services.AddSingleton<TokenService>(); // vai vai durar para sempre uma instancia para toda aplicação
var app = builder.Build();

app.MapControllers();

app.Run();
