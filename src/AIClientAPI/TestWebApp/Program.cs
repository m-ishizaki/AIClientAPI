using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mvcBuilder = builder.Services.AddControllersWithViews();

// dll 読み込み
{
    var basePath = Path.Combine(Directory.GetParent(Assembly.GetEntryAssembly()!.Location!)!.Parent!.Parent!.Parent!.Parent!.FullName, "AIClientAPI\\bin");
    var files = Directory.GetFiles(basePath, "*.dll", SearchOption.AllDirectories).Select(dll => Assembly.LoadFrom(dll)).ToArray();
    foreach (var assembly in files)
    {
        mvcBuilder.AddApplicationPart(assembly);
    }
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
