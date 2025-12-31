using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;
using AspNet.Security.OAuth.Discord;
using ClosingTime.Data;
using ClosingTime.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

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

    options.Scope.Add("guilds");
    options.Scope.Add("guilds.members.read");

    options.Events.OnCreatingTicket = async context =>
    {
        // 1. Get user info from the raw JSON payload (context.User)
    var userData = JsonNode.Parse(context.User.GetRawText());
    var discordId = userData?["id"]?.GetValue<string>();
    var name = userData?["username"]?.GetValue<string>();
    var avatar = userData?["avatar"]?.GetValue<string>();

    // 2. Setup Guild Member Check
    string guildId = "963448945592373320";
    var request = new HttpRequestMessage(HttpMethod.Get, 
        $"https://discord.com/api/v10/users/@me/guilds/{guildId}/member");
    
    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
    var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);

    // 3. Database Sync (UpdateOrCreate)
    var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
    ulong dId = ulong.Parse(discordId!);
    
    var user = await db.Users.FirstOrDefaultAsync(u => u.DiscordId == dId);
    if (user == null) {
        user = new ApplicationUser { DiscordId = dId };
        db.Users.Add(user);
    }

        user.Username = name;
        user.AvatarHash = avatar;

        // 4. Role & Whitelist Logic
        if (response.IsSuccessStatusCode)
        {
            var memberData = JsonNode.Parse(await response.Content.ReadAsStringAsync());
            var nickname = memberData?["nick"]?.GetValue<string>() ?? name;
            var roles = memberData?["roles"]?.AsArray();

            // Admin Role IDs
            var adminRoles = new[] { "963496000935301181", "971022818055696384" };
            bool isAdmin = roles?.Any(r => adminRoles.Contains(r?.GetValue<string>())) ?? false;

            user.DisplayName = nickname; // Use server nick if present
            user.IsWhitelisted = true;
            user.IsAdmin = isAdmin;
        }
        else
        {
            user.IsWhitelisted = false;
            user.IsAdmin = false;
        }

        await db.SaveChangesAsync();

        var identity = context.Principal?.Identity as ClaimsIdentity;
        
        string bestName = user.DisplayName ?? user.Username ?? "Unknown";
        identity?.AddClaim(new Claim("DisplayName", bestName));
        identity?.AddClaim(new Claim("AvatarHash", avatar));
        identity?.AddClaim(new Claim("Admin", user.IsAdmin.ToString().ToLower()));
        identity?.AddClaim(new Claim("Whitelisted", user.IsWhitelisted.ToString().ToLower()));
    };
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
