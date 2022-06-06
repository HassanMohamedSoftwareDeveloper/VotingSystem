using Microsoft.EntityFrameworkCore;
using VotingSystem;
using VotingSystem.Application;
using VotingSystem.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("Cookie")
    .AddCookie("Cookie");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("Database");
});

builder.Services.AddSingleton<ICounterManager, CounterManager>();
builder.Services.AddSingleton<IVotingPollFactory, VotingPollFactory>();
builder.Services.AddScoped<VotingInteractor>();
builder.Services.AddScoped<StatisticsInteractor>();
builder.Services.AddScoped<VotingPollInteractor>();
builder.Services.AddScoped<IVotingSystemPersistence, VotingSystemPersistence>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapDefaultControllerRoute();

app.Run();
