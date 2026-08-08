<#
    provjeri.ps1 - dokazuje da su TESTOVI ispravni, ne da je kod ispravan.

    Svaki zadatak mora proci dvije provjere:

      1. RJESENJE ZELENO  - testovi se pokrenu protiv referentnog rjesenja
                            i svi moraju proci. Dokazuje da test uopce ima
                            rjesenje (nema nemoguce tvrdnje, krivog iznosa...).

      2. KOSTUR CRVEN     - isti testovi protiv nedovrsenog kostura i barem
                            jedan mora pasti. Dokazuje da test nije prazan;
                            test koji prolazi na praznom kodu ne uci nicemu.

    Uz to: nijedan test PONASANJA (naziv ne pocinje s "Gradja_") ne smije
    proci na kosturu. Testovi gradje smiju - na razini A gradja je poklonjena.

    Pokretanje:
        pwsh ./provjeri.ps1
        pwsh ./provjeri.ps1 -Obrazac Strategija
#>

[CmdletBinding()]
param(
    [string]$Obrazac = "",
    [string]$Dotnet  = "dotnet"
)

$ErrorActionPreference = "Stop"
$korijen = Split-Path -Parent $MyInvocation.MyCommand.Path
$sln     = Join-Path $korijen "Rppoon.Practice.sln"
$izlaz   = Join-Path $korijen ".provjera"

if (Test-Path $izlaz) { Remove-Item $izlaz -Recurse -Force }
New-Item -ItemType Directory -Path $izlaz | Out-Null

function Pokreni-Testove {
    param([string]$Naziv, [string[]]$Dodatno)

    $trx = Join-Path $izlaz "$Naziv.trx"
    $argumenti = @("test", $sln, "--nologo", "-v", "q",
                   "--logger", "trx;LogFileName=$trx") + $Dodatno
    if ($Obrazac) { $argumenti += @("--filter", "TestCategory=$Obrazac") }

    Write-Host "  pokrecem: $Naziv ..." -ForegroundColor DarkGray
    & $Dotnet @argumenti *> (Join-Path $izlaz "$Naziv.log")

    if (-not (Test-Path $trx)) {
        Write-Host "GRESKA: prevodenje nije uspjelo ($Naziv). Ispis:" -ForegroundColor Red
        Get-Content (Join-Path $izlaz "$Naziv.log") | Select-Object -Last 40
        exit 2
    }
    return $trx
}

function Ucitaj-Rezultate {
    param([string]$Trx)

    [xml]$xml = Get-Content $Trx -Raw
    $ns = @{ t = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010" }

    # testId -> "Razred.Metoda"
    $imena = @{}
    foreach ($def in $xml.TestRun.TestDefinitions.UnitTest) {
        $razred = ($def.TestMethod.className -split "\.")[-1]
        $imena[$def.id] = "$razred.$($def.TestMethod.name)"
    }

    $rez = @{}
    foreach ($r in $xml.TestRun.Results.UnitTestResult) {
        $puno = $imena[$r.testId]
        if ($puno) { $rez[$puno] = $r.outcome }
    }
    return $rez
}

Write-Host ""
Write-Host "RPPOON - provjera ispravnosti testova" -ForegroundColor Cyan
Write-Host "======================================"

$trxRjesenja = Pokreni-Testove -Naziv "rjesenja" -Dodatno @("-p:Rjesenja=true")
$trxKostur   = Pokreni-Testove -Naziv "kostur"   -Dodatno @()

$rjesenja = Ucitaj-Rezultate $trxRjesenja
$kostur   = Ucitaj-Rezultate $trxKostur

# Grupiraj po razredu testova (jedan razred = jedan zadatak).
$zadaci = ($rjesenja.Keys + $kostur.Keys | ForEach-Object { ($_ -split "\.")[0] }) |
          Sort-Object -Unique

$redci = @()
$pao   = $false

foreach ($zadatak in $zadaci) {
    $uRjesenju = $rjesenja.GetEnumerator() | Where-Object { $_.Key -like "$zadatak.*" }
    $uKosturu  = $kostur.GetEnumerator()   | Where-Object { $_.Key -like "$zadatak.*" }

    $rjesenjeZeleno = ($uRjesenju | Where-Object { $_.Value -ne "Passed" }).Count -eq 0
    $kosturCrven    = ($uKosturu  | Where-Object { $_.Value -ne "Passed" }).Count -ge 1

    # Test ponasanja koji prolazi na praznom kosturu = test bez sadrzaja.
    $prazni = $uKosturu | Where-Object {
        $_.Value -eq "Passed" -and ($_.Key -split "\.")[1] -notlike "Gradja_*"
    } | ForEach-Object { ($_.Key -split "\.")[1] }

    $ok = $rjesenjeZeleno -and $kosturCrven -and ($prazni.Count -eq 0)
    if (-not $ok) { $pao = $true }

    $redci += [pscustomobject]@{
        Zadatak  = $zadatak
        Testova  = ($uRjesenju | Measure-Object).Count
        Rjesenje = if ($rjesenjeZeleno) { "zeleno" } else { "PALO" }
        Kostur   = if ($kosturCrven)    { "crven"  } else { "PROLAZI!" }
        Prazni   = if ($prazni.Count -eq 0) { "-" } else { ($prazni -join ", ") }
        Status   = if ($ok) { "OK" } else { "GRESKA" }
    }
}

$redci | Format-Table -AutoSize

if ($pao) {
    Write-Host "NEISPRAVNI ZADACI - popravi testove prije dijeljenja." -ForegroundColor Red
    Write-Host "  'Rjesenje: PALO'   -> test trazi nesto sto ni rjesenje ne zadovoljava."
    Write-Host "  'Kostur: PROLAZI!' -> testovi prolaze i na nedovrsenom kodu."
    Write-Host "  'Prazni: ...'      -> ti testovi ponasanja nista ne provjeravaju."
    exit 1
}

Write-Host "Svi zadaci ispravni: rjesenje zeleno, kostur crven." -ForegroundColor Green
exit 0
