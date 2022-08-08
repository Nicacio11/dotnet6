using System.Text.Json.Serialization;
using Blog.Configurations;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();
builder.AddAuthentication();
builder.Services.AddServices();

//adiciona o cache de memoria
builder.Services.AddMemoryCache();

builder.Services.AddControllers()
.ConfigureApiBehaviorOptions(options => 
{
    options.SuppressModelStateInvalidFilter = true;
})
.AddJsonOptions(x => 
{
    // configuration to ignore cycles reference
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

    //ignore null object
    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
});

var app = builder.Build();


//configurando autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
