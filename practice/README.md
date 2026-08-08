# RPPOON — vježbe s NUnit testovima

Zadaci za uvježbavanje 23 oblikovna obrasca. Ne pišeš `Main` ni ispis —
pišeš razrede dok testovi ne postanu zeleni.

## Pokretanje

```bash
dotnet test practice/Rppoon.Practice.sln          # tvoj kod
dotnet test practice/Rppoon.Practice.sln -p:Rjesenja=true   # referentna rješenja
```

Samo jedan obrazac:

```bash
dotnet test practice/Rppoon.Practice.sln --filter "TestCategory=Strategija"
```

Samo jedna razina težine:

```bash
dotnet test practice/Rppoon.Practice.sln --filter "TestCategory=C"
```

## Tri načina rada — jedan ne traži nikakvu instalaciju

| Način | Što treba instalirati | Kome odgovara |
|---|---|---|
| **GitHub Codespaces** | ništa | *Code → Codespaces → Create*. Dobiješ VS Code u pregledniku s već instaliranim .NET-om. |
| **Lokalno** | .NET SDK 8 | najbrža petlja; `winget install Microsoft.DotNet.SDK.8`, pa `dotnet test` |
| **Fork + push** | ništa | GitHub Actions pokrene testove i pokaže zeleno/crveno |

Visual Studio **nije** potreban — projekti su SDK-style i rade iz terminala
na Windowsu, Linuxu i macOS-u.

## Što sam riješio?

```powershell
pwsh ./practice/napredak.ps1
```

Ispisuje sva 23 obrasca redom kojim se obrađuju na predavanjima, i za svaki
šest zadataka:

```
STVARANJE
Obrazac            A1  A2  B1  B2  C1  C2
Singleton          [+] [+] [~] [ ] [ ] [ ]
MetodaTvornica      .   .   .   .   .   .   jos nije pripremljen
```

`[+]` riješeno · `[~]` djelomično · `[ ]` nezapočeto · `.` zadatak još ne postoji

Ne moraš ići redom — svaki zadatak stoji sam za sebe:

```bash
pwsh ./practice/napredak.ps1 -Skupina Struktura
pwsh ./practice/napredak.ps1 -Obrazac Singleton
dotnet test --filter "TestCategory=Stvaranje&TestCategory=A"
```

## Kako je posloženo

Obrasci su grupirani kao na predavanjima — **stvaranje → struktura → ponašanje**:

```
practice/
  src/Rppoon.Zadaci/
    Zadaci/1_Stvaranje/Singleton/      <- OVDJE PIŠEŠ
    Zadaci/3_Ponasanje/Strategija/
    Rjesenja/…                         <- referentno rješenje (nakon pokušaja)
    RjesenjaAlt/…                      <- isto riješeno drukčije
  tests/Rppoon.Testovi/1_Stvaranje/…   <- testovi; njih ne diraš
  napredak.ps1                         <- što si riješio
  provjeri.ps1                         <- jesu li testovi ispravni
```

Obje mape koriste **iste** prostore imena i nazive tipova, pa se isti testovi
prevode protiv bilo koje inačice. Prekidač `-p:Rjesenja=true` bira koju.

## Tri razine težine

| Razina | Što dobiješ | Što pišeš |
|---|---|---|
| **A — vođeno** | cijela građa: sučelja, kontekst, potpisi | 2–3 tijela metoda označena s `// TODO` |
| **B — sastavi** | sučelje i potpisi razreda | cijelu implementaciju i povezivanje |
| **C — ispitno** | samo tekst zadatka i tražene nazive | sve, od prazne datoteke |

Testovi razine C ne referenciraju tvoje tipove pri prevođenju nego ih traže
**odrazom** (reflection) — zato smiješ početi od prazne datoteke. Cijena je da
nazivi moraju biti točno onakvi kakve zadatak traži. Svaki tip mora biti
`public`, inače ga testni projekt ne vidi.

## Zašto testovi provjeravaju i građu, a ne samo rezultat

Zadatak „sortiraj rastuće" možeš riješiti jednim `list.Sort()` i test bi
prošao — a obrazac Strategija ne bi bio primijenjen. Zato uz ponašanje
provjeravamo i građu:

| Obrazac | Strukturna provjera |
|---|---|
| Singleton | konstruktor nije javan; `Instance` vraća istu referencu |
| Strategija | kontekst drži član tipa sučelja, ne grana po vrsti |
| Dekorater | dekorater implementira sučelje **i** drži član tog tipa (omata, ne nasljeđuje) |
| Adapter | tip `Adaptee` nema referencu na ciljno sučelje |
| Kompozit | list i kompozit dijele bazni tip |
| Promatrač | odjava stvarno prestane slati obavijesti |

Ti testovi nose prefiks `Gradja_`.

## „A što ako riješim drukčije nego autor?"

Prava opasnost ovakvih zadataka: test skrojen točno po autorovu rješenju, pa
student koji obrazac primijeni **ispravno ali drukčije** — padne. Protiv toga
svaki zadatak ima **dva neovisno napisana rješenja**, i testovi moraju
prihvatiti oba:

```bash
dotnet test practice/Rppoon.Practice.sln -p:Rjesenja=true   # referentno
dotnet test practice/Rppoon.Practice.sln -p:Rjesenja=alt    # napisano drukčije
```

Primjer — isti zadatak, dva puta:

| | referentno | alternativno |
|---|---|---|
| postotni popust | `amount - amount * p / 100` | množitelj `1 - p/100` u konstruktoru |
| zbroj stavki | `foreach` petlja | `prices.Sum()` |
| kontekst drži strategiju | automatsko svojstvo | izričito polje + `get/set` |
| provjera znamenke | `foreach` + `char.IsDigit` | `input.Any(char.IsDigit)` |

Ako oba prolaze, test provjerava **obrazac**, a ne autorov stil.

Testovi su k tome popustljivi gdje zadatak ništa nije propisao: `Total()` i
`Total { get; }` jednako vrijede, svojstvo i javno polje jednako vrijede.
Strogi su samo ondje gdje tekst zadatka izričito traži naziv ili potpis.

## Kako znam da su testovi ispravni

```powershell
pwsh ./practice/provjeri.ps1
```

Svaki zadatak prolazi kroz **tri** pokretanja istih testova:

1. **rješenje zeleno** — protiv referentnog rješenja svi prolaze
   (dokaz da zadatak uopće ima rješenje: nema nemoguće tvrdnje, krivog iznosa)
2. **alternativa zelena** — protiv drukčije napisanog rješenja svi prolaze
   (dokaz da test nije prilijepljen uz jedan način pisanja)
3. **kostur crven** — protiv nedovršenog kostura barem jedan pada
   (dokaz da test nije prazan — test koji prolazi na praznom kodu ništa ne uči)

Pada i ako neki test **ponašanja** (naziv ne počinje s `Gradja_`) prolazi na
praznom kosturu. Testovi građe smiju proći — na razini A građa je poklonjena.

Ako zadatak nema alternativno rješenje, skripta to **prijavi** umjesto da tiho
javi zeleno — inače bi drugo pokretanje vrtjelo isti kod i ništa ne dokazivalo.

Isto se vrti u GitHub Actions pri svakom `push`-u.
