# ============================================================
#  CafeLux - Deploy Local voi ngrok
#  Chay: .\deploy-local.ps1
# ============================================================

$ErrorActionPreference = "Continue"
$ProjectRoot = $PSScriptRoot

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "   CAFELUX - DEPLOY LOCAL VIA NGROK" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# ---- Ham cho port mo ----
function Wait-ForPort {
    param([int]$Port, [string]$Name, [int]$TimeoutSeconds = 120)
    Write-Host "    > Dang cho $Name tren port $Port..." -ForegroundColor Gray
    $elapsed = 0
    while ($elapsed -lt $TimeoutSeconds) {
        try {
            $tcp = New-Object System.Net.Sockets.TcpClient
            $tcp.Connect("127.0.0.1", $Port)
            $tcp.Close()
            Write-Host "    > $Name san sang! ($elapsed giay)" -ForegroundColor Green
            return $true
        } catch {
            Start-Sleep -Seconds 2
            $elapsed += 2
            Write-Host "    > Cho... ($elapsed giay)" -ForegroundColor DarkGray
        }
    }
    Write-Host "    > [LOI] $Name khong khoi dong sau $TimeoutSeconds giay!" -ForegroundColor Red
    return $false
}

# ---- Kiem tra ngrok ----
$ngrokPath = (Get-Command ngrok -ErrorAction SilentlyContinue)
if (-not $ngrokPath) {
    $candidates = @(
        "$env:LOCALAPPDATA\Microsoft\WinGet\Packages\Ngrok.Ngrok_Microsoft.Winget.Source_8wekyb3d8bbwe\ngrok.exe",
        "$env:USERPROFILE\AppData\Local\Programs\Ngrok\ngrok.exe",
        "C:\ProgramData\chocolatey\bin\ngrok.exe"
    )
    $found = $candidates | Where-Object { Test-Path $_ } | Select-Object -First 1
    if ($found) {
        $ngrokCmd = $found
    } else {
        Write-Host "[LOI] Khong tim thay ngrok. Tai tai: https://ngrok.com/download" -ForegroundColor Red
        Read-Host "Nhan Enter de thoat"
        exit 1
    }
} else {
    $ngrokCmd = "ngrok"
}

# ---- [1/4] Dung tien trinh cu ----
Write-Host "[1/4] Dung cac tien trinh cu (neu co)..." -ForegroundColor Yellow
Get-Process -Name "ngrok" -ErrorAction SilentlyContinue | Stop-Process -Force
Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
Start-Sleep -Seconds 2

# ---- [2/4] Khoi dong API ----
Write-Host ""
Write-Host "[2/4] Khoi dong Coffee API (port 5098, HTTP)..." -ForegroundColor Yellow

$apiProcess = Start-Process -FilePath "dotnet" `
    -ArgumentList "run --project `"$ProjectRoot\Coffee.API\Coffee.API.csproj`" --launch-profile http" `
    -WorkingDirectory "$ProjectRoot\Coffee.API" `
    -PassThru `
    -WindowStyle Minimized

Write-Host "    > API PID: $($apiProcess.Id)" -ForegroundColor Gray
$apiReady = Wait-ForPort -Port 5098 -Name "Coffee API"
if (-not $apiReady) {
    Write-Host "    [CANH BAO] API chua san sang, tiep tuc anyway..." -ForegroundColor DarkYellow
}

# ---- [3/4] Khoi dong MVC ----
Write-Host ""
Write-Host "[3/4] Khoi dong Coffee MVC (port 5210, HTTP)..." -ForegroundColor Yellow

$mvcProcess = Start-Process -FilePath "dotnet" `
    -ArgumentList "run --project `"$ProjectRoot\Coffee\Coffee.csproj`" --launch-profile http" `
    -WorkingDirectory "$ProjectRoot\Coffee" `
    -PassThru `
    -WindowStyle Minimized

Write-Host "    > MVC PID: $($mvcProcess.Id)" -ForegroundColor Gray
$mvcReady = Wait-ForPort -Port 5210 -Name "Coffee MVC"
if (-not $mvcReady) {
    Write-Host "[LOI] MVC khong khoi dong duoc. Dung lai." -ForegroundColor Red
    Stop-Process -Id $apiProcess.Id -Force -ErrorAction SilentlyContinue
    exit 1
}

# ---- [4/4] Khoi dong ngrok ----
Write-Host ""
Write-Host "[4/4] Khoi dong ngrok tunnel cho MVC (port 5210)..." -ForegroundColor Yellow

$ngrokProcess = Start-Process -FilePath $ngrokCmd `
    -ArgumentList "http 5210 --host-header=rewrite" `
    -PassThru `
    -WindowStyle Minimized

# Cho ngrok ket noi va lay URL tu API
Start-Sleep -Seconds 4
$publicUrl = $null
for ($i = 0; $i -lt 15; $i++) {
    try {
        $tunnels = Invoke-RestMethod -Uri "http://localhost:4040/api/tunnels" -ErrorAction Stop
        $publicUrl = ($tunnels.tunnels | Where-Object { $_.proto -eq "https" } | Select-Object -First 1).public_url
        if ($publicUrl) { break }
    } catch {}
    Start-Sleep -Seconds 2
}

Write-Host ""
Write-Host "============================================" -ForegroundColor Green
Write-Host "  CAFELUX DANG CHAY!" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Green
Write-Host ""
Write-Host "  Local  (API) : http://localhost:5098" -ForegroundColor White
Write-Host "  Local  (Web) : http://localhost:5210" -ForegroundColor White
Write-Host ""
if ($publicUrl) {
    Write-Host "  *** LINK CONG KHAI - GUI CHO NGUOI KHAC ***" -ForegroundColor Yellow
    Write-Host "  $publicUrl" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "  Ngrok Dashboard : http://localhost:4040" -ForegroundColor Gray
} else {
    Write-Host "  [!] Khong lay duoc URL. Vao http://localhost:4040 de xem" -ForegroundColor DarkYellow
}
Write-Host ""
Write-Host "  Nhan Enter de DUNG tat ca" -ForegroundColor Yellow
Write-Host ""

Read-Host ">> Nhan Enter de dung"

# Dung tat ca
Write-Host ""
Write-Host "Dang dung cac tien trinh..." -ForegroundColor Yellow
Stop-Process -Id $ngrokProcess.Id -Force -ErrorAction SilentlyContinue
Stop-Process -Id $apiProcess.Id -Force -ErrorAction SilentlyContinue
Stop-Process -Id $mvcProcess.Id -Force -ErrorAction SilentlyContinue
Get-Process -Name "ngrok" -ErrorAction SilentlyContinue | Stop-Process -Force
Write-Host "Da dung tat ca." -ForegroundColor Green

