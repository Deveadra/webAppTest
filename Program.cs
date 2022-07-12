using Blog.Data;
using Blog.Data.FileManager;
using Blog.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    //options.SignIn.RequireConfirmedAccount = true;
    
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 2;

})

    
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
});

builder.Services.AddTransient<IRepository, Repository>();
builder.Services.AddTransient<IFileManager, FileManager>();

//invoke policy Cross-Origin Resource Sharing
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            );
});


builder.Services.AddMvc(options => options.EnableEndpointRouting = false);


builder.Services.Configure<IdentityOptions>(options =>
{
    //    //PASSWORD SETUP
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 2;
    options.Password.RequiredUniqueChars = 1;

    //    //LOCKOUT SETUP
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    //    //USER SETUP
    options.User.RequireUniqueEmail = true;

});


//COOKIE SETUP
builder.Services.ConfigureApplicationCookie(options =>
{

    options.Cookie.HttpOnly = true;
    //options.Cookie.Expiration = TimeSpan.FromDays(30);
    options.ExpireTimeSpan = TimeSpan.FromDays(30);


    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

//ROLE SETUP

var srv = builder.Services.BuildServiceProvider();
var ctx = srv.GetRequiredService<ApplicationDbContext>();
////handles all user accounts
var userMgr = srv.GetRequiredService<UserManager<IdentityUser>>();
////handles all the roles you can assign to users
var roleMgr = srv.GetRequiredService<RoleManager<IdentityRole>>();

ctx.Database.EnsureCreated();

var adminRole = new IdentityRole("Admin");

if (!ctx.Roles.Any())
{
    //create a role
    roleMgr.CreateAsync(adminRole).GetAwaiter().GetResult();

}

if (!ctx.Users.Any(u => u.UserName == "admin"))
{
    //create an admin
    //userMgr.CreateAsync(IdentityUser).GetAwaiter().GetResult();
    var adminUser = new IdentityUser
    {
        UserName = "admin",
        Email = "admin@test.com"
    };
    //might not work, try assigning to a var instead
    var result = userMgr.CreateAsync(adminUser, "Password1").GetAwaiter().GetResult();
    //add role to user
    userMgr.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();

}

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
    app.UseDatabaseErrorPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//must be invoked before HttpsRedirection
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    //endpoints.MapRazorPages();
});

app.MapRazorPages();

app.Run();
