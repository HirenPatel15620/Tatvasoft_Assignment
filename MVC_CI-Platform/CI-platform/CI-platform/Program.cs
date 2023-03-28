using Microsoft.EntityFrameworkCore;
using CI.DataAcess.Repository.IRepository;
using CI.DataAcess.Repository;
using CI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddAuthentication("AuthCookie").AddCookie("AuthCookie", options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(2);
    options.Cookie.Name = "AuthCookie";
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/LogOut";
});

builder.Services.AddDbContext<CiPlatformContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("SqlConnectionString")
    ));
builder.Services.AddScoped<IAllRepository,AllRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=User}/{controller=UserAuthentication}/{action=Login}");
//app.MapControllerRoute(
//    name: "UserAuthentication",
//    pattern: "{controller=UserAuthentication}/{action=Login}"
//    );
app.Run();
