using AIUI2025.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PatternDashboard.Data;
using Microsoft.Extensions.Options;
using FXAI.Models;

var builder = WebApplication.CreateBuilder(args);

Stat.Url = builder.Configuration.GetSection("ApiSettings").GetSection("MarketDataUrl").Value;

builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings"));

builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<ApiSettings>>().Value);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddHttpClient<MarketDataService>();
builder.Services.AddScoped<CandleBuilder>();
builder.Services.AddSingleton<PatternService>();
builder.Services.AddSingleton<TickService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
