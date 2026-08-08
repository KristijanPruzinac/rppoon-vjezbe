<#
    provjeri.ps1 - dokazuje da su TESTOVI ispravni, ne da je kod ispravan.

    Svaki zadatak prolazi kroz tri pokretanja istih testova:

      1. RJESENJE ZELENO   - protiv referentnog rjesenja svi moraju proci.
                             Dokazuje da zadatak IMA rjesenje: nema nemoguce
                             tvrdnje, krivo izracunatog iznosa, tipfelera.

      2. ALTERNATIVA ZELENA - protiv drugacije napisanog rjesenja svi moraju
                             proci. Dokazuje da test provjerava OBRAZAC, a ne
                             moj nacin pisanja. Bez ovoga bi student koji je
                             obrazac primijenio ispravno ali drugacije - pao.

      3. KOSTUR CRVEN      - protiv nedovrsenog kostura barem jedan mora pasti.
                             Dokazuje da test nije prazan; test koji prolazi na
                             praznom kodu nicemu ne uci.

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
$mapaAlt = Join-Path $korijen "src/Rppoon.Zadaci/RjesenjaAlt"
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

    # testId -> "Obrazac/Razred/Metoda"
    $imena = @{}
    foreach ($def in $xml.TestRun.TestDefinitions.UnitTest) {
        $dijelovi = $def.TestMethod.className -split "\."
        $razred   = $dijelovi[-1]
        $obrazac  = if ($dijelovi.Length -ge 2) { $dijelovi[-2] } else { "?" }
        $imena[$def.id] = "$obrazac/$razred/$($def.TestMethod.name)"
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
$trxAlt      = Pokreni-Testove -Naziv "alt"      -Dodatno @("-p:Rjesenja=alt")
$trxKostur   = Pokreni-Testove -Naziv "kostur"   -Dodatno @()

$rjesenja = Ucitaj-Rezultate $trxRjesenja
$alt      = Ucitaj-Rezultate $trxAlt
$kostur   = Ucitaj-Rezultate $trxKostur

# Kljuc zadatka je "Obrazac/Razred", npr. "Strategija/C1_Testovi".
$zadaci = @($rjesenja.Keys) + @($kostur.Keys) |
          ForEach-Object { ($_ -split "/")[0] + "/" + ($_ -split "/")[1] } |
          Sort-Object -Unique

$redci    = @()
$pao      = $false
$bezAlta  = 0

foreach ($zadatak in $zadaci) {
    $uRjesenju = $rjesenja.GetEnumerator() | Where-Object { $_.Key -like "$zadatak/*" }
    $uAltu     = $alt.GetEnumerator()      | Where-Object { $_.Key -like "$zadatak/*" }
    $uKosturu  = $kostur.GetEnumerator()   | Where-Object { $_.Key -like "$zadatak/*" }

    $obrazacZadatka = ($zadatak -split "/")[0]
    $prefiks        = (($zadatak -split "/")[1] -split "_")[0]   # "C1_Testovi" -> "C1"

    # Postoji li STVARNO alternativno rjesenje za ovaj zadatak? Ako ne postoji,
    # prolaz u drugom pokretanju nista ne dokazuje - vrtio se isti kod.
    $imaAlt = $false
    if (Test-Path $mapaAlt) {
        $imaAlt = @(Get-ChildItem -Path (Join-Path $mapaAlt $obrazacZadatka) `
                                  -Filter "$($prefiks)_*.cs" -ErrorAction SilentlyContinue).Count -gt 0
    }

    $rjesenjeZeleno = ($uRjesenju | Where-Object { $_.Value -ne "Passed" }).Count -eq 0
    $kosturCrven    = ($uKosturu  | Where-Object { $_.Value -ne "Passed" }).Count -ge 1

    $prazni = @($uKosturu | Where-Object {
        $_.Value -eq "Passed" -and ($_.Key -split "/")[2] -notlike "Gradja_*"
    } | ForEach-Object { ($_.Key -split "/")[2] })

    $altPali = @($uAltu | Where-Object { $_.Value -ne "Passed" } |
                 ForEach-Object { ($_.Key -split "/")[2] })

    if (-not $imaAlt) { $bezAlta++ }

    $altStatus = if (-not $imaAlt)          { "nema" }
                 elseif ($altPali.Count -eq 0) { "zeleno" }
                 else                       { "PALO: " + ($altPali -join ", ") }

    $ok = $rjesenjeZeleno -and $kosturCrven -and ($prazni.Count -eq 0) -and
          (-not ($imaAlt -and $altPali.Count -gt 0))
    if (-not $ok) { $pao = $true }

    $redci += [pscustomobject]@{
        Zadatak     = $zadatak
        Testova     = @($uRjesenju).Count
        Rjesenje    = if ($rjesenjeZeleno) { "zeleno" } else { "PALO" }
        Alternativa = $altStatus
        Kostur      = if ($kosturCrven)    { "crven"  } else { "PROLAZI!" }
        Prazni      = if ($prazni.Count -eq 0) { "-" } else { ($prazni -join ", ") }
        Status      = if ($ok) { "OK" } else { "GRESKA" }
    }
}

$redci | Format-Table -AutoSize

if ($pao) {
    Write-Host "NEISPRAVNI ZADACI - popravi testove prije dijeljenja." -ForegroundColor Red
    Write-Host "  'Rjesenje: PALO'    -> test trazi nesto sto ni rjesenje ne zadovoljava."
    Write-Host "  'Alternativa: PALO' -> test je skrojen po jednom rjesenju; student koji"
    Write-Host "                         obrazac primijeni ispravno ali drugacije - pada."
    Write-Host "  'Kostur: PROLAZI!'  -> testovi prolaze i na nedovrsenom kodu."
    Write-Host "  'Prazni: ...'       -> ti testovi ponasanja nista ne provjeravaju."
    exit 1
}

if ($bezAlta -gt 0) {
    Write-Host "UPOZORENJE: $bezAlta zadatak/zadataka nema alternativno rjesenje." -ForegroundColor Yellow
    Write-Host "  Za njih je provjereno samo da rjesenje prolazi i kostur pada, ali NE i"
    Write-Host "  da test prihvaca drugaciji nacin pisanja. Dodaj datoteku u"
    Write-Host "  src/Rppoon.Zadaci/RjesenjaAlt/<Obrazac>/ s istim nazivom kao u Rjesenja/."
}

Write-Host "Svi zadaci ispravni: rjesenje zeleno, kostur crven." -ForegroundColor Green
exit 0
