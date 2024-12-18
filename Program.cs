using LeaveManagementSystem.Application;
using LeaveManagementSystem.Application.Services.Email;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

DataServiceRegistration.AddDataServices(builder.Services, builder.Configuration);
ApplicationServiceRegistration.AddApplicationServices(builder.Services);

builder.Host.UseSerilog((ctx, config)=>
config.WriteTo.Console()
.ReadFrom.Configuration(ctx.Configuration)
);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOrSupervisorOnly", policy =>
    {
        policy.RequireRole(Roles.Administrator, Roles.Supervisor);
    });
});




builder.Services.AddHttpContextAccessor();



builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
