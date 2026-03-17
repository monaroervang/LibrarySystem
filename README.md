## LibrarySystem – LINQ & EF Core

Mona Rørvang

Jeg har valgt MSSQL fremfor SQLite, da det er en server-baseret database og jeg vil gerne øve mig på realistisk .NET udvikling. Serverbaseret givre bedre queryoptimering samt kan håndtere større datamængder.
Større datamængder vil jeg mene er relevant i et administrationssystem til et bibliotek.


## Sådan køres projektet

1. Opret databasen via EF Core migrations

```
dotnet ef database update
```

2. Kør Console-applikationen

```
dotnet run
```

Programmet vil derefter udskrive resultaterne af de forskellige LINQ queries i konsollen.

## Udfordringer

En udfordring under udviklingen var, at EF Core ikke altid kunne oversætte LINQ til SQL. F.eks. ved queries med `GroupBy`, beregninger eller sortering på DTO.

Løsninger: Oprette en anonym type først, sortering, og derefter projicere til en DTO til sidst. Ved beregning af værdier som f.eks. `(ReturnDate - LoanDate).Days` hentede jeg data fra databasen, hvorefter beregningen blev udført i C# ved hjælp af `AsEnumerable()`.

Dog fik jeg en bedre forståelse af når LINQ skal oversættes til SQL eller til objekter i C#.

Og selvfølgelig tiden. Jeg kunne have delt DbContext bedre op med Config til hver entitet eller lavet en QueryService i stedet for at have hele koden i Program.cs, lavet adresse eller fullname som VO (value obejcts). Eller også interface og repository til f.eks. Loan.
