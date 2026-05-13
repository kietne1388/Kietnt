Write-Host "Stopping all Coffee processes..." -ForegroundColor Yellow

# Kill all dotnet processes related to Coffee
Get-Process | Where-Object { $_.ProcessName -eq "dotnet" } | ForEach-Object {
    Write-Host "Stopping process: $($_.Id)" -ForegroundColor Cyan
    Stop-Process -Id $_.Id -Force -ErrorAction SilentlyContinue
}

Write-Host "All processes stopped." -ForegroundColor Green

