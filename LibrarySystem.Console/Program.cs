using LibrarySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using LibrarySystem.Console.DTOs;
using System.Linq;

using var context = new LibraryDbContext();

Console.WriteLine("\n=== SEED CHECK (Counts) ===");
Console.WriteLine($"Authors:   {context.Authors.Count()}");
Console.WriteLine($"Books:     {context.Books.Count()}");
Console.WriteLine($"Borrowers: {context.Borrowers.Count()}");
Console.WriteLine($"Loans:     {context.Loans.Count()}");


//-----------------------------------------------------------DEL 2--------------------------------------------------------------------------------------------

//Hent alle låneres fulde navn og email
var borrowers = context.Borrowers //Repræsenterer hele Borrowers-tabellen
    .Select(b => new BorrowerDto(
        b.FirstName + " " + b.LastName,
        b.Email
    ))
    .ToList();//Her kører query. Jeg bruger ToList() fordi, at jeg skal bruge resultatet i en foreach

Console.WriteLine("Lånere:");
foreach (var b in borrowers) //Vis listen
{
    Console.WriteLine($"Navn: {b.FullName}  E-mail: {b.Email}");
}

//Hent alle aktive lån
var activeLoans = context.Loans //Repræsenterer hele Loans-tabellen
    .AsNoTracking()//Performance boost til læsning
    .Where(l => l.ReturnDate == null)
    .Select(l => new ActiveLoanDto(
        l.Id,
        l.Borrower.FirstName + " " + l.Borrower.LastName, //Select laver automatisk JOIN ved at projicere
        l.Book.Title,//Select laver automatisk JOIN ved at projicere
        l.LoanDate,
        l.DueDate
    ))
    .ToList();

Console.WriteLine("\nAktive lån:");
foreach (var loan in activeLoans)
{
    Console.WriteLine($"{loan.LoanId}: {loan.BorrowerName} Udlån: {loan.BookTitle} Afleveres senest: {loan.DueDate:d}");
}
/* Herunder et eksempel på samme output, men med SelectMany i stedet for JOIN
 var activeLoans = context.Borrowers
    .AsNoTracking()
    .SelectMany(b => b.Loans //Fladgører Borrowers
        .Where(l => l.ReturnDate == null)
        .Select(l => new ActiveLoanDto(
            l.Id,
            b.FirstName + " " + b.LastName,
            l.Book.Title,
            l.LoanDate,
            l.DueDate
        )))
    .ToList();
 */

//-----------------------------------------------------------DEL 3--------------------------------------------------------------------------------------------

//Bøger sorterte efter udgivelsesår
var sortedBooks = context.Books //Repræsenterer hele Books-tabellen
    .AsNoTracking()//Performance boost til læsning
    .OrderByDescending(b => b.PublicationYear)//Sortér efter nyeste først
    .Select(b => new BookSortedDto(
        b.Id,
        b.Title,
        b.PublicationYear
    ))
    .ToList();

Console.WriteLine("\nBøger sorteret (nyeste først):");
foreach (var book in sortedBooks)
{
    Console.WriteLine($"U.år: {book.PublicationYear} Titel: {book.Title} (Id: {book.Id})");
}

//Find alle bøger med "Harry" i titlen
var harryBooks = context.Books
    .AsNoTracking()
    .Where(b => b.Title.Contains("Harry"))
    .Select(b => new BookSearchDto(
        b.Id,
        b.Title,
        b.PublicationYear
    ))
    .ToList();

Console.WriteLine("\nBøger med 'Harry' i titlen:");
foreach (var book in harryBooks)
{
    Console.WriteLine($"Id: {book.Id} Titel: {book.Title} År: {book.PublicationYear}");
}

//Aktive udlån total
var totalLoans = context.Loans.Count();
var activeLoanCount = context.Loans.Count(l => l.ReturnDate == null);

Console.WriteLine($"\nAntal udlån i alt: {totalLoans}");
Console.WriteLine($"Antal aktive udlån: {activeLoanCount}");

//Udlån med bogtitel og lånernavn
var loanOverview = context.Loans
    .AsNoTracking() //Readonly for hurtigere performance
    .Select(l => new LoanOverviewDto( //Ved brug af Select laver EF automatisk JOIN
        l.Id,
        l.Borrower.FirstName + " " + l.Borrower.LastName,//JOIN
        l.Book.Title,//JOIN
        l.LoanDate
    ))
    .ToList();//Her kører query. LINQ = .Dump();

Console.WriteLine("\nAlle udlån:");
foreach (var loan in loanOverview)
{
    Console.WriteLine($"{loan.LoanId}: {loan.BorrowerName} lånte '{loan.BookTitle}' d. {loan.LoanDate:d}");
}

/*
 //Samme query, men med .Include
var loanOverview = context.Loans
    .AsNoTracking()
    .Include(l => l.Borrower) //Loans, Borrower og Book hentes som objekter og samles derefter i DTO
    .Include(l => l.Book)
    .Select(l => new LoanOverviewDto( //Dog projicerer EF stadig, så ovenstående er lidt "overkill"
        l.Id,
        l.Borrower.FirstName + " " + l.Borrower.LastName,
        l.Book.Title,
        l.LoanDate
    ))
    .ToList();

//Her med JOIN
var loanOverview = context.Loans
    .AsNoTracking()
    .Join(context.Borrowers,
        l => l.BorrowerId,
        b => b.Id,
        (l, b) => new { l, b })
    .Join(context.Books,
        lb => lb.l.BookId,
        book => book.Id,
        (lb, book) => new LoanOverviewDto(
            lb.l.Id,
            lb.b.FirstName + " " + lb.b.LastName,
            book.Title,
            lb.l.LoanDate
        ))
    .ToList();

 */


//Forfattere med antal bøger
var authorBookCounts = context.Books
    .AsNoTracking()
    .GroupBy(b => new { b.AuthorId, b.Author.FirstName, b.Author.LastName })//Grupperer efter forfatter
    .Select(g => new AuthorBookCountDto(
        g.Key.AuthorId,
        g.Key.FirstName + " " + g.Key.LastName,
        g.Count()//Antal bøger
    ))
    .ToList();
/*Jeg prøvede at lave en OrderByDescending EFTER sortering, men pga. min DTO midt i sorteringen kan EF ikke oversætte til SQL. Løsningen kunne være, at DTO'en først bliver lavet til sidst:
.GroupBy
.Select
.OrderByDescending
.Select
.ToList*/

Console.WriteLine("\nForfattere og antal bøger:");
foreach (var a in authorBookCounts)
{
    Console.WriteLine($"{a.AuthorName}: {a.BookCount} bøger");
}

//Udlån bog, forfatter, låner
var loanDetails = context.Loans
    .AsNoTracking()
    .Select(l => new LoanDetailsDto(
        l.Id,
        l.Borrower.FirstName + " " + l.Borrower.LastName,//EF projicere ved Select og laver dermed automatiks JOIN Borrower
        l.Book.Title,//JOIN Book
        l.Book.Author.FirstName + " " + l.Book.Author.LastName,//JOIN Author
        l.LoanDate,
        l.DueDate
    ))
    .ToList();

Console.WriteLine("\nUdlånsdetaljer:");
foreach (var l in loanDetails)
{
    Console.WriteLine($"{l.LoanId}: {l.BorrowerName} lånte '{l.BookTitle}' af {l.AuthorName} d. {l.LoanDate:d} Afleveringsfrist: {l.DueDate:d}");
}


//Bøger der har været udlånt mere end to gange
var popularBooks = context.Loans
    .AsNoTracking()
    .GroupBy(l => new { l.BookId, l.Book.Title })//Grupperer efter bøger
    .Select(g => new { g.Key.BookId, g.Key.Title, LoanCount = g.Count() })//Tæller antal udlån pr. bog. EF kan ikke oversætte min DTO til SQL inde i en sortering
    .Where(x => x.LoanCount > 2)//Hvor Count er mere end 2
    .Select(x => new BookLoanCountDto(x.BookId, x.Title, x.LoanCount))//DTO laves
    .ToList();//Først her køres SQL

Console.WriteLine("\nBøger udlånt mere end to gange:");
foreach (var b in popularBooks)
{
    Console.WriteLine($"Id: {b.BookId} {b.Title} er udlånt {b.LoanCount} gange");
}//Ingen af mine bøger i min database var lånt ud mere end to gange, så jeg har ændret seed-data i LibraryDbContext

//-----------------------------------------------------------DEL 4 + 5--------------------------------------------------------------------------------------------

//Antal udlån pr. måned i 2026
var loansPerMonth = context.Loans
    .AsNoTracking()
    .Where(l => l.LoanDate.Year == 2026) //Vælger år 2026
    .GroupBy(l => new { l.LoanDate.Year, l.LoanDate.Month })//Gruppering på måned
    .Select(g => new LoansPerMonthDto( //g = gruppen og Key = værdien jeg grupperede på. g.Key bliver derfor til gruppen med to properties (her er det Year og Month)
        g.Key.Year, //Året for gruppen
        g.Key.Month, //Måneden for gruppen
        g.Count()//Aggregat der tæller lån for gruppen
    ))
    .ToList();

Console.WriteLine("\nUdlån pr. måned (2026):");
foreach (var l in loansPerMonth)
{
    Console.WriteLine($"{l.Year} - {l.Month}: {l.LoanCount} lån");
}

//Gennemsnitlig låneperiode for afleverede bøger
var avgDays = context.Loans
    .AsNoTracking()
    .Where(l => l.ReturnDate != null)//Kun afleverede bøger. Hvis jeg allerede brugte .AsEnumerable her, ville det forringe performance, da ALLE lån hentes i databasen først
    .AsEnumerable()//.Date og .Days er udtrukket af DateTime, og det kan EF ikke oversætte til SQL. Derfor beder jeg kun SQL om at hente data, stopper den og kører og beregner resten i C#
    .Average(l => (l.ReturnDate!.Value.Date - l.LoanDate.Date).Days);

Console.WriteLine($"\nGennemsnitlig låneperiode (afleverede): {avgDays:F1} dage"); //Ingen foreach og derfor ingen ToList();

//Top 3 for bøger, der er udlånt mest
var topBooks = context.Loans
    .AsNoTracking()//Igen, højere performance, tracker ikke data, men læser, mapper til DTO og printer ud
    .GroupBy(l => new { l.BookId, l.Book.Title }) //Gruppering per bog
    .Select(g => new
    {
        g.Key.BookId,
        g.Key.Title,
        LoanCount = g.Count()
    })
    .OrderByDescending(x => x.LoanCount)//De mest udlånte først
    .Take(3)//Kun de 3 øverste
    .Select(x => new TopBookDto(x.BookId, x.Title, x.LoanCount))//DTO oprettes
    .ToList();

Console.WriteLine("\nTop 3 mest populære bøger:");
foreach (var b in topBooks)
{
    Console.WriteLine($"{b.Title} - {b.LoanCount} udlån");
}

//Lånerstatistik
var borrowerStats = context.Loans
    .AsNoTracking()
    .GroupBy(l => new { l.BorrowerId, l.Borrower.FirstName, l.Borrower.LastName })
    .Select(g => new BorrowerLoanStatsDto(
        g.Key.BorrowerId,
        g.Key.FirstName + " " + g.Key.LastName,
        g.Count(l => l.ReturnDate == null),//Betinget aggregat, som er aggregater, hvor der skal beregnes på noget bestemt. Tæller kun hvor ReturnDate = null (aktive lån)
        g.Count(l => l.ReturnDate != null),//Betinget aggregat, der kun tæller ReturnDate !=null, altså afleverede lån
        g.Count()//Total af de to ovenstående
    ))
    .ToList();

Console.WriteLine("\nLånerstatistik:");
foreach (var s in borrowerStats)
{
    Console.WriteLine($"{s.BorrowerName}: Aktive = {s.ActiveLoans}, Afleverede = {s.ReturnedLoans}, Total = {s.TotalLoans}");
}

//Lån der har overskredet afleveringsfrist
var overdueLoans = context.Loans
    .AsNoTracking()
    .Where(l => l.ReturnDate == null && l.DueDate < DateTime.Now) //Server evaluering: Filtrering sker i DB, så kun relevante lån hentes. EF laver automatisk Join pga. navigation properties. IQueryable = SQL
    .Select(l => new
    {
        l.Id,
        BorrowerName = l.Borrower.FirstName + " " + l.Borrower.LastName,
        BookTitle = l.Book.Title,
        l.DueDate
    })
    .ToList()
    //Materialisering: SQL udføres her (Deferred execution).
    //EF kan ikke oversætte DateTime med .Date og .Days til SQL, derfor hentes data til ToList()
    //Herefter beregning i C#, som er client evaluering.
    .Select(x => new OverdueLoanDto(
        x.Id,
        x.BorrowerName,
        x.BookTitle,
        x.DueDate,
        (DateTime.Now.Date - x.DueDate.Date).Days//Beregning af antal dage for forsinket aflevering
    ))
    .ToList(); //Her køres og beregnes i C#

Console.WriteLine("\nOverskredne lån:");
foreach (var o in overdueLoans)
{
    Console.WriteLine($"{o.LoanId}: {o.BorrowerName} har lånt '{o.BookTitle}' ({o.DaysLate} dage forsinket, afleveringsfrist d. {o.DueDate:d})");
}