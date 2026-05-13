$currentDir = Get-Location

Write-Host "Starting Coffee System..." -ForegroundColor Green

# First, stop any existing processes
Write-Host "Stopping existing processes..." -ForegroundColor Yellow
Get-Process | Where-Object { $_.ProcessName -eq "dotnet" } | ForEach-Object {
    Stop-Process -Id $_.Id -Force -ErrorAction SilentlyContinue
}
Start-Sleep -Seconds 2

# Start API
Write-Host "Launching API..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "dotnet run --project Coffee.API/Coffee.API.csproj --launch-profile https" -WorkingDirectory $currentDir

# Wait for API to initialize
Start-Sleep -Seconds 5

# Start MVC
Write-Host "Launching MVC App..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "dotnet run --project Coffee/Coffee.csproj --launch-profile https" -WorkingDirectory $currentDir

Write-Host "Done. Check the two new PowerShell windows." -ForegroundColor Green
