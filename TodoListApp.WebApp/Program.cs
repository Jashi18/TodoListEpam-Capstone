using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Services;
using TodoListApp.Services.Database;
using TodoListApp.Services.Database.Services;
using TodoListApp.Services.WebApi;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoListDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var configuration = builder.Configuration;

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<TodoListDbContext>();

builder.Services.AddHttpClient<TodoListWebApiService>(client =>
{
    client.BaseAddress = new Uri(configuration["ApiSettings:BaseUri"]);
});

builder.Services.AddHttpClient<TaskWebApiService>(client =>
{
    client.BaseAddress = new Uri(configuration["ApiSettings:BaseUri"]);
});

builder.Services.AddHttpClient<CommentWebApiService>(client =>
{
    client.BaseAddress = new Uri(configuration["ApiSettings:BaseUri"]);
});
builder.Services.AddHttpClient<TagWebApiService>(client =>
{
    client.BaseAddress = new Uri(configuration["ApiSettings:BaseUri"]);
});


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

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

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
