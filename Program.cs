using Blog.Configurations;
var builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();
builder.AddAuthentication();
builder.Services.AddServices();


builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => 
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();


//configurando autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
