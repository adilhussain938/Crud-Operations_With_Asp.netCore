using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using BulkyBook.Models;
using Bulkybook.DataAccess;
using Bulkybook.DataAccess.Repository.IRepository;
using Bulkybook.DataAccess.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using BulkyBook.Utility;
//using Microsoft.AspNet.Identity.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DBContextsol>(options => options.UseSqlServer
(
    builder.Configuration.GetConnectionString("DefaultCont")
));

builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<DBContextsol>();
builder.Services.AddScoped<IUnitOfwork, UnitOfWork>();
builder.Services.AddSingleton<IEmailSender, EmailaSender>();
builder.Services.AddMvc();


builder.Services.ConfigureApplicationCookie(
   options => 
   { 
      options.LoginPath = $"/Identity/Account/Login";
       options.LogoutPath = $"/Identity/Account/Logout";
   }); 


var app = builder.Build(); 

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages(); //Routes for pages
    endpoints.MapControllers(); //Routes for my API controllers
});
//app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
