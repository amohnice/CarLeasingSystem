using CarLeasingSystem.Data;
using Microsoft.AspNetCore.Identity; // 1. Necessary for IdentityUser
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- SERVICES CONFIGURATION ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Add Identity services
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // 3. Required for Identity UI pages

var app = builder.Build();

// --- HTTP REQUEST PIPELINE ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Ensure this is present
app.UseRouting();

// 4. IMPORTANT: Authentication must come BEFORE Authorization
app.UseAuthentication(); 
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// 5. Map the Identity Razor Pages
app.MapRazorPages(); 

app.Run();
