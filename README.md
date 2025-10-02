# My Tasks - ToDo MVC Application

## Opis projekta

Ovo je ASP.NET Core MVC aplikacija za upravljanje zadacima (ToDo list).  
Omogucava korisnicima registraciju, prijavu, dodavanje, izmenu i brisanje zadataka. Projekat koristi ASP.NET Core Identity za autentifikaciju i autorizaciju.

## Uputstvo za pokretanje

## 1. Pokretanje lokalno
1. Kloniraj repozitorijum:

    git clone https://github.com/David-Stepanic/ToDoApp.git
    
2. Instaliraj potrebne NuGet pakete:

    dotnet restore
    
3. Kreiraj ili azuriraj bazu:

    dotnet ef database update
    
4. Pokreni aplikaciju:

    cd ToDo
    dotnet run
    
5. Otvori u browser-u:
    
    https://localhost:5175

## Arhitekturni pregled

Aplikacija je razvijena koristeci ASP.NET Core MVC arhitekturu sa slojevima:

- Models – definicija podataka (ToDo, Category, Status, ApplicationUser).
- Views – Razor stranice za prikaz podataka.
- Controllers – upravljanje HTTP zahtevima (HomeController, AccountController).
- Services – logika poslovanja (ToDoService).
- Repositories – pristup podacima kroz Entity Framework Core.
- Identity – autentifikacija i autorizacija korisnika.

Glavni tok aplikacije:
1. Korisnik se registruje/prijavljuje.
2. Dodaje, uredjuje ili brise zadatke.
3. Podaci se cuvaju u SQL Server bazi preko Entity Framework Core.
4. UI koristi Bootstrap i jQuery za bolje korisničko iskustvo.

---

## Ključne odluke i kompromisi

1. **Identity integracija**  
   - Izabrano je ASP.NET Core Identity zbog jednostavnog i sigurnog upravljanja korisnicima.
   - Kompromis: povećana složenost baze i dodatne tabele.

2. **Entity Framework Core**  
   - Omogucava jednostavan rad sa bazom, migracijama i seed podacima.
   - Kompromis: nesto sporiji pristup u poređenju sa ručnim SQL upitima.

3. **MVC arhitektura**  
   - Jasna separacija odgovornosti (Model-View-Controller).
   - Kompromis: kod je ponekad rasprsen na vise fajlova.

4. **Bootstrap + jQuery za UI**  
   - Brzo i jednostavno za razvoj interfejsa.
   - Kompromis: veca velicina stranice i potencijalno manja kontrola nad performansama.

---

## Struktura projekta

/Controllers
/Models
/Views
/wwwroot
/css
/js
/Services
/Repositories
appsettings.json
Program.cs
Startup.cs
README.md



