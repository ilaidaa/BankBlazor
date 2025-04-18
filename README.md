# BankBlazor – Examinationsuppgift i Systemutveckling .NET

## Beskrivning
Detta projekt är en del av den examinerande inlämningen i kursen *Systemutveckling .NET*. Syftet är att bygga en bankapplikation enligt **headless-arkitektur**, där frontend (Blazor WebAssembly) och backend (.NET Web API) är separerade och kommunicerar med varandra via **JSON** och **HTTP-anrop**.
Backend är kopplad till en databas genom **Entity Framework Core (Database First)**.


## Funktionalitet
### Backend (ASP.NET Core Web API)
- Hämtar kundprofiler och konton via endpoints
- Visar saldo och kundinformation
- Utför transaktioner: **insättning**, **uttag** och **överföring mellan konton**
- Alla transaktioner uppdateras och sparas i databasen
### Frontend (Blazor WebAssembly)
- Gränssnitt för att:
  Söka och visa en **kundprofil**
  Göra **insättningar**, **uttag** och **överföringar**
- Navigering mellan tre sidor: **Home**, **Customer Profile** och **Transactions**
- All data hämtas från API:et via HTTP-anrop och visas direkt i gränssnittet


## Setup
### Connection String till databasen:
Server=localhost;Database=BankBlazor;Trusted_Connection=True;TrustServerCertificate=true;Command Timeout=180
**Database First Scaffolding:** Server=localhost;Database=BankBlazor;Trusted_Connection=True;TrustServerCertificate=true;Command Timeout=180" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Data


## Krav som uppfylls
- Headless-arkitektur (.NET API + Blazor WebAssembly)
- Entity Framework Core Database First
- API-endpoints för att hämta konton, saldo och göra transaktioner
- Transaktioner sparas i databasen
- Blazor-klient för att hantera transaktioner och kundprofil
- Navigering mellan tre sidor
- Konsumtion av eget API via HTTP-anrop och JSON
- Användning av GIT och feature branches under utveckling


## Struktur
- **BankBlazor.API** – Backendprojektet med API, Controllers och Data-modeller
- **BankBlazor.Client** – Blazor WebAssembly-klienten med sidor och design
- **README.md** – Dokumentationen du läser nu


## Tekniker & Verktyg
- Blazor WebAssembly
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Git & GitHub
- Visual Studio 2022
  


