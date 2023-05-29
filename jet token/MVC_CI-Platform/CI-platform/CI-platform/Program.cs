using Microsoft.EntityFrameworkCore;
using CI.Repository.Repository.IRepository;
using CI.Repository.Repository;
using CI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<CiPlatformContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("SqlConnectionString")
    ));
builder.Services.AddScoped<IAllRepository,AllRepository>();



//builder.Services.AddAuthentication("AuthCookie").AddCookie("AuthCookie", options =>
//{
//    options.ExpireTimeSpan = TimeSpan.FromDays(2);
//    options.Cookie.Name = "AuthCookie";
//    options.LoginPath = "/Auth/Login";
//    options.LogoutPath = "/Auth/LogOut";
//});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSetting:Issuer"],
        ValidAudience = builder.Configuration["JwtSetting:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSetting:Key"]))
    };
});


//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromDays(1);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});
//builder.Services.AddAuthorization(options => {
//    options.AddPolicy("SuperUserRights", policy => policy.RequireRole("Admin", "SuperUser", "BackupAdmin"));
//});
//builder.Services.AddAuthorization(option =>
//{
//    option.AddPolicy("AdminOnly", policy =>
//    {
//        policy.RequireRole("Role", "Admin");
//    });
//    option.AddPolicy("UserOnly", policy =>
//    {
//        policy.RequireRole("Role", "User");
//    });
//});
builder.Services.AddMvc().AddSessionStateTempDataProvider();
builder.Services.AddAuthorization();
builder.Services.AddSession();
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
app.UseSession();
app.Use(async (context, next) =>
{
    var token = context.Session.GetString("Token");
    if (!string.IsNullOrWhiteSpace(token))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + token);
    }
    await next();
});

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
