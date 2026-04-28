using System.IO;
using Microsoft.EntityFrameworkCore;
using SchoolAppCore.Models;
using FastReport.Web;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolDBConnectionString")));

builder.Services.AddFastReport();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

//app.Use(async (context, next) =>
//{
//    if (context.Request.Path.StartsWithSegments("/_content/FastReport.Web", StringComparison.OrdinalIgnoreCase, out var remaining))
//    {
//        // Redirect browser to the physical open-source Razor Class Library folder
//        context.Response.Redirect("/_content/FastReport.OpenSource.Web" + remaining, permanent: false);
//        return; // Important: Return immediately to send the redirect response
//    }
//    await next();
//});
var rewriteOptions = new RewriteOptions()
    .AddRewrite("^_content/FastReport\\.Web/(.*)", "_content/FastReport.OpenSource.Web/$1", skipRemainingRules: true);
app.UseRewriter(rewriteOptions);
app.UseStaticFiles();

app.UseRouting();

// FastReport middleware MUST be placed after app.UseRouting()
app.UseFastReport();

app.UseAuthorization();

var reportsDirectory = new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "Reports"));
if (!reportsDirectory.Exists)
{
    reportsDirectory.Create();
}

app.MapRazorPages();

app.Run();