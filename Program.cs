using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebAppSignalR.Contexts;
using WebAppSignalR.Hubs;
using WebAppSignalR.Models.Services;

var builder = WebApplication.CreateBuilder(args);












// Add services to the container.
var mvcBuilder = builder.Services.AddControllersWithViews();

#if DEBUG
mvcBuilder.AddRazorRuntimeCompilation();
#endif

// signalR
builder.Services.AddSignalR();
string connectionString = "";
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IChatRoomService, ChatRoomService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddAuthentication(options =>
options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{ options.LoginPath = "/Home/Login"; });













var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();



// for signalR
app.UseAuthentication();




app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// signalR
app.MapHub<SiteChatHub>("/chathub");
app.MapHub<SupportHub>("/supporthub");

app.Run();
