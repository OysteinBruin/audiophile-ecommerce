using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebReactSpa;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();


builder.Services.AddNextjsStaticHosting();

var app = builder.Build();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");



app.MapNextjsStaticHtmls();
app.UseNextjsStaticHosting();

app.Run();