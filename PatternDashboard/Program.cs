using AIUI2025.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PatternDashboard.Data;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

Stat.Url = builder.Configuration.GetSection("ApiSettings").GetSection("MarketDataUrl").Value;

builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings"));

builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<ApiSettings>>().Value);
var basePath = Path.Combine(AppContext.BaseDirectory, "CandleData");
var accountId = builder.Configuration["Oanda:AccountId"];
var token = builder.Configuration["Oanda:AccessToken"];
var isPractice = bool.Parse(builder.Configuration["Oanda:IsPractice"] ?? "true");
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddHttpClient<MarketDataService>();
builder.Services.AddScoped<CandleBuilder>();
builder.Services.AddSingleton<TickService>();
builder.Services.AddScoped<BacktestService>();
builder.Services.AddScoped<ITradeSimulator, TradeSimulator>();

builder.Services.AddScoped<ICandleRepository, CandleRepository>();
builder.Services.AddScoped<IPatternService, PatternService>();

builder.Services.AddScoped<CandleStore>(provider => new CandleStore(basePath));

Stat.Oanda = new OandaCandleService(new CandleStore(basePath), accountId, token, isPractice);

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
