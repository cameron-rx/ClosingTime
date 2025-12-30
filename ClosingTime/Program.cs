using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/login";
})
.AddDiscord(options =>
{
    options.ClientId = builder.Configuration["Discord:ClientId"];
    options.ClientSecret = builder.Configuration["Discord:ClientSecret"];
    options.SaveTokens = true; 
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");  
    options.Conventions.AllowAnonymousToPage("/login"); 
});

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.PageViewLocationFormats.Add("/Pages/Shared/Layouts/{0}" + RazorViewEngine.ViewExtension);
    options.PageViewLocationFormats.Add("/Pages/Shared/Partials/{0}" + RazorViewEngine.ViewExtension);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();
app.Run();
