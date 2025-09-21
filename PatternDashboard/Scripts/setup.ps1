# This script scaffolds Project X
# setup.ps1 — Scaffolds folder structure and starter files for Project X

$root = "AIUI2025"
$folders = @(
    "$root/PatternDashboard",
    "$root/Services",
    "$root/Models",
    "$root/Scripts",
    "$root/wwwroot/js",
    "$root/wwwroot/css",
    "$root/Shared",
    "$root/Data",
    "$root/Pages"
)

$files = @{
    "$root/PatternDashboard/PatternDashboard.razor" = "<h3>Pattern Dashboard</h3>"
    "$root/PatternDashboard/MarketChart.razor" = "<div id='chartContainer'></div>"
    "$root/Services/PatternService.cs" = "// Pattern extraction logic"
    "$root/Services/MarketDataService.cs" = "// Tick/candle data provider"
    "$root/Models/Candle.cs" = "public class Candle { public DateTime Time; public double Open, High, Low, Close; }"
    "$root/wwwroot/js/chartInterop.js" = "// Highcharts JS interop"
    "$root/Scripts/setup.ps1" = "# This script scaffolds Project X"
    "$root/Data/mock-candles.json" = "[{ \"time\": \"2025-09-21T09:00:00Z\", \"open\": 100, \"high\": 105, \"low\": 95, \"close\": 102 }]"
}

foreach ($folder in $folders) {
    New-Item -ItemType Directory -Path $folder -Force | Out-Null
}

foreach ($path in $files.Keys) {
    Set-Content -Path $path -Value $files[$path] -Force
}

Write-Host "✅ Project X folder structure and starter files created."