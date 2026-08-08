<#
    napredak.ps1 - koliko si zadataka rijesio.

    Pokrene testove protiv TVOG koda i pokaze tablicu po skupinama
    obrazaca. Nista ne mijenja i nista ne otkriva - samo status.

        pwsh ./napredak.ps1
        pwsh ./napredak.ps1 -Skupina Stvaranje
        pwsh ./napredak.ps1 -Obrazac Singleton

    Oznake:
        [+]  svi testovi zadatka prolaze
        [~]  dio prolazi (obicno su gotovi testovi gradje, ne i ponasanja)
        [ ]  jos nista
#>

[CmdletBinding()]
param(
    [ValidateSet("", "Stvaranje", "Struktura", "Ponasanje")]
    [string]$Skupina = "",
    [string]$Obrazac = "",
    [string]$Dotnet  = "dotnet"
)

$ErrorActionPreference = "Stop"
$korijen = Split-Path -Parent $MyInvocation.MyCommand.Path
$sln     = Join-Path $korijen "Rppoon.Practice.sln"
$izlaz   = Join-Path $korijen ".provjera"

# Redoslijed kao na predavanjima: stvaranje -> struktura -> ponasanje.
$poredak = [ordered]@{
    "Stvaranje" = @("Singleton", "MetodaTvornica", "ApstraktnaTvornica", "Graditelj", "Prototip")
    "Struktura" = @("Adapter", "Most", "Kompozit", "Dekorater", "Fasada", "Muha", "Proxy")
    "Ponasanje" = @("LanacOdgovornosti", "Naredba", "Iterator", "Posrednik", "Memento",
                    "Promatrac", "Stanje", "Strategija", "PredlozakMetode", "Posjetitelj", "NullObjekt")
}

if (-not (Test-Path $izlaz)) { New-Item -ItemType Directory -Path $izlaz | Out-Null }
$trx = Join-Path $izlaz "napredak.trx"
if (Test-Path $trx) { Remove-Item $trx -Force }

$argumenti = @("test", $sln, "--nologo", "-v", "q", "--logger", "trx;LogFileName=$trx")
$filtar = @()
if ($Skupina) { $filtar += "TestCategory=$Skupina" }
if ($Obrazac) { $filtar += "TestCategory=$Obrazac" }
if ($filtar.Count -gt 0) { $argumenti += @("--filter", ($filtar -join "&")) }

Write-Host ""
Write-Host "racunam napredak ..." -ForegroundColor DarkGray
& $Dotnet @argumenti *> (Join-Path $izlaz "napredak.log")

if (-not (Test-Path $trx)) {
    Write-Host "Kod se ne prevodi. Zadnjih 30 redaka ispisa:" -ForegroundColor Red
    Get-Content (Join-Path $izlaz "napredak.log") | Select-Object -Last 30
    exit 2
}

[xml]$xml = Get-Content $trx -Raw

$imena = @{}
foreach ($def in $xml.TestRun.TestDefinitions.UnitTest) {
    $dijelovi = $def.TestMethod.className -split "\."
    $imena[$def.id] = @{ Obrazac = $dijelovi[-2]; Zadatak = ($dijelovi[-1] -split "_")[0] }
}

# "Obrazac/Zadatak" -> @{ Prosli; Ukupno }
$stanje = @{}
foreach ($r in $xml.TestRun.Results.UnitTestResult) {
    $info = $imena[$r.testId]
    if (-not $info) { continue }

    $kljuc = "$($info.Obrazac)/$($info.Zadatak)"
    if (-not $stanje.ContainsKey($kljuc)) { $stanje[$kljuc] = @{ Prosli = 0; Ukupno = 0 } }

    $stanje[$kljuc].Ukupno++
    if ($r.outcome -eq "Passed") { $stanje[$kljuc].Prosli++ }
}

function Oznaka {
    param([string]$Kljuc)
    if (-not $stanje.ContainsKey($Kljuc)) { return $null }
    $s = $stanje[$Kljuc]
    if ($s.Prosli -eq $s.Ukupno) { return "[+]" }
    if ($s.Prosli -eq 0)         { return "[ ]" }
    return "[~]"
}

Write-Host ""
Write-Host "RPPOON - napredak" -ForegroundColor Cyan
Write-Host "================="

$ukupnoRijeseno = 0
$ukupnoDostupno = 0

foreach ($skupinaNaziv in $poredak.Keys) {
    if ($Skupina -and $skupinaNaziv -ne $Skupina) { continue }

    $redci = @()
    foreach ($obrazacNaziv in $poredak[$skupinaNaziv]) {
        if ($Obrazac -and $obrazacNaziv -ne $Obrazac) { continue }

        $celije = [ordered]@{ Obrazac = $obrazacNaziv }
        $imaIkakav = $false

        foreach ($zadatak in @("A1", "A2", "B1", "B2", "C1", "C2")) {
            $oznaka = Oznaka "$obrazacNaziv/$zadatak"
            if ($null -eq $oznaka) {
                $celije[$zadatak] = " . "        # zadatak jos nije napisan
            }
            else {
                $imaIkakav = $true
                $ukupnoDostupno++
                if ($oznaka -eq "[+]") { $ukupnoRijeseno++ }
                $celije[$zadatak] = $oznaka
            }
        }

        $celije["stanje"] = if ($imaIkakav) { "" } else { "jos nije pripremljen" }
        $redci += [pscustomobject]$celije
    }

    if ($redci.Count -gt 0) {
        Write-Host ""
        Write-Host $skupinaNaziv.ToUpper() -ForegroundColor Yellow
        $redci | Format-Table -AutoSize
    }
}

Write-Host ""
if ($ukupnoDostupno -gt 0) {
    $postotak = [math]::Round(100.0 * $ukupnoRijeseno / $ukupnoDostupno)
    Write-Host "Rijeseno: $ukupnoRijeseno / $ukupnoDostupno dostupnih zadataka ($postotak %)" -ForegroundColor Green
}
Write-Host "Legenda:  [+] rijeseno   [~] djelomicno   [ ] nezapoceto   ' . ' zadatak jos nije pripremljen"
Write-Host ""
Write-Host "Radi na jednom zadatku:" -ForegroundColor DarkGray
Write-Host "  dotnet test --filter `"TestCategory=Singleton`""
Write-Host "  dotnet test --filter `"TestCategory=Stvaranje&TestCategory=A`""
