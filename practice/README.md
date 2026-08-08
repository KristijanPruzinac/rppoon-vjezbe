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

## Kako je posloženo

```
practice/
  src/Rppoon.Zadaci/
    Zadaci/<Obrazac>/     <- OVDJE PIŠEŠ
    Rjesenja/<Obrazac>/   <- referentno rješenje (pogledaj tek nakon pokušaja)
  tests/Rppoon.Testovi/
    <Obrazac>/            <- testovi; njih ne diraš
  provjeri.ps1
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

## Kako znam da su testovi ispravni

```powershell
pwsh ./practice/provjeri.ps1
```

Svaki zadatak mora proći dvije provjere:

1. **rješenje zeleno** — testovi protiv referentnog rješenja svi prolaze
   (dokaz da zadatak uopće ima rješenje: nema nemoguće tvrdnje, krivog iznosa)
2. **kostur crven** — isti testovi protiv nedovršenog kostura, barem jedan pada
   (dokaz da test nije prazan — test koji prolazi na praznom kodu ništa ne uči)

Dodatno pada ako neki test **ponašanja** (naziv ne počinje s `Gradja_`) prolazi
na praznom kosturu. Testovi građe smiju proći — na razini A građa je poklonjena.

Isto se vrti u GitHub Actions pri svakom `push`-u.
