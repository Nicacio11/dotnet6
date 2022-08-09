using System.IO.Compression;
using System.Text.Json.Serialization;
using Blog.Configurations;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();
builder.AddAuthentication();
builder.Services.AddServices();

//adiciona o cache de memoria
builder.Services.AddMemoryCache();

builder.Services.AddResponseCompression(options => 
{
    //options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    //options.Providers.Add<CustomCompressionProvider>();
});
builder.Services.Configure<GzipCompressionProviderOptions>(options => 
{
    options.Level = CompressionLevel.Optimal;
});
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
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.UseResponseCompression();

app.Run();
