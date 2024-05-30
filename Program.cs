using MichaelKjellander.Components;
using MichaelKjellander.SharedUtils;

var builder = WebApplication.CreateBuilder(args);

//Console.WriteLine("**** args="+args+"; "+args.Length+"; "+args[0]);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddHttpClient();

builder.Services.Configure<AppConfig>(config =>
{
    config.IsDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
    config.SiteUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")!;
});

var app = builder.Build();


app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();




// var builder = WebApplication.CreateBuilder(args);
//
// // Add services to the container.
// builder.Services.AddRazorComponents()
//     .AddInteractiveServerComponents();
//
// var app = builder.Build();
//
// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Error", createScopeForErrors: true);
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }
//
// app.UseHttpsRedirection();
//
// app.UseStaticFiles();
// app.UseAntiforgery();
//
// app.MapRazorComponents<App>()
//     .AddInteractiveServerRenderMode();
//
// app.Run();