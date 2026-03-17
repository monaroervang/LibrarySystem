using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
namespace LibrarySystem.Infrastructure.Data;

//DbContext håndterer mapping og kommunikation mellem entities og databasen (ORM)
public class LibraryDbContext : DbContext
{
    //DbSet = svarer til tabeller i databasen
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Borrower> Borrowers => Set<Borrower>();
    public DbSet<Loan> Loans => Set<Loan>();

    //OnConfiguring angiver connection-string
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
    }

    //Kaldes af EF Core ved opbygning af entity-modellen
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);//Kalder base. Standard EF setup

        // Konfiguration af relationer mellem entiteterne
        modelBuilder.Entity<Author>()
            .HasMany(a => a.Books)//Author 1 - M Book
            .WithOne(b => b.Author)//Hver Book har 1 Author
            .HasForeignKey(b => b.AuthorId);//FK i Book

        modelBuilder.Entity<Book>()
            .HasMany(b => b.Loans)//Book 1 - M Loan
            .WithOne(l => l.Book)
            .HasForeignKey(l => l.BookId);//FK i Loan

        modelBuilder.Entity<Borrower>()
            .HasMany(br => br.Loans)//Borrower 1 - M Loan
            .WithOne(l => l.Borrower)
            .HasForeignKey(l => l.BorrowerId);// FK i Loan

        modelBuilder.Entity<Author>()
            .Navigation(a => a.Books)
            .UsePropertyAccessMode(PropertyAccessMode.Field); //Backing fields. Fortæller EF, at det skal bruge dte private field bagved

        modelBuilder.Entity<Book>()
            .Navigation(b => b.Loans)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        modelBuilder.Entity<Borrower>()
            .Navigation(br => br.Loans)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        //Seed-data (start-data). HasData bliver f.eks. via migrations til INSERT i SQL
        modelBuilder.Entity<Author>().HasData(
            new { Id = 1, FirstName = "J.K.", LastName = "Rowling", Nationality = "UK", BirthYear = 1965 },
            new { Id = 2, FirstName = "George", LastName = "Orwell", Nationality = "UK", BirthYear = 1903 },
            new { Id = 3, FirstName = "Karen", LastName = "Blixen", Nationality = "DK", BirthYear = 1885 },
            new { Id = 4, FirstName = "Stephen", LastName = "King", Nationality = "US", BirthYear = 1947 },
            new { Id = 5, FirstName = "Suzanne", LastName = "Collins", Nationality = "US", BirthYear = 1962 }
        );

        modelBuilder.Entity<Book>().HasData(
            new { Id = 1, Title = "Harry Potter 1", PublicationYear = 1997, Pages = 223, Genre = "Fantasy", Isbn = "9780747532699", AuthorId = 1 },
            new { Id = 2, Title = "Harry Potter 2", PublicationYear = 1998, Pages = 251, Genre = "Fantasy", Isbn = "9780747538493", AuthorId = 1 },
            new { Id = 3, Title = "1984", PublicationYear = 1949, Pages = 328, Genre = "Dystopia", Isbn = "9780451524935", AuthorId = 2 },
            new { Id = 4, Title = "Animal Farm", PublicationYear = 1945, Pages = 112, Genre = "Satire", Isbn = "9780451526342", AuthorId = 2 },
            new { Id = 5, Title = "Out of Africa", PublicationYear = 1937, Pages = 416, Genre = "Memoir", Isbn = "9780141183332", AuthorId = 3 },
            new { Id = 6, Title = "The Shining", PublicationYear = 1977, Pages = 447, Genre = "Horror", Isbn = "9780307743657", AuthorId = 4 },
            new { Id = 7, Title = "IT", PublicationYear = 1986, Pages = 1138, Genre = "Horror", Isbn = "9781501142970", AuthorId = 4 },
            new { Id = 8, Title = "The Hunger Games", PublicationYear = 2008, Pages = 374, Genre = "YA", Isbn = "9780439023528", AuthorId = 5 },
            new { Id = 9, Title = "Catching Fire", PublicationYear = 2009, Pages = 391, Genre = "YA", Isbn = "9780439023498", AuthorId = 5 },
            new { Id = 10, Title = "Mockingjay", PublicationYear = 2010, Pages = 390, Genre = "YA", Isbn = "9780439023511", AuthorId = 5 }
        );

        modelBuilder.Entity<Borrower>().HasData(
            new { Id = 1, FirstName = "Anna", LastName = "Jensen", Email = "anna@demo.dk", Phone = "11111111", Address = "Testvej 1" },
            new { Id = 2, FirstName = "Mikkel", LastName = "Hansen", Email = "mikkel@demo.dk", Phone = "22222222", Address = "Testvej 2" },
            new { Id = 3, FirstName = "Sara", LastName = "Nielsen", Email = "sara@demo.dk", Phone = "33333333", Address = "Testvej 3" },
            new { Id = 4, FirstName = "Jonas", LastName = "Pedersen", Email = "jonas@demo.dk", Phone = "44444444", Address = "Testvej 4" }
        );

        modelBuilder.Entity<Loan>().HasData(
            new { Id = 1, BookId = 1, BorrowerId = 1, LoanDate = new DateTime(2026, 1, 1), DueDate = new DateTime(2026, 1, 31), ReturnDate = (DateTime?)null },
            new { Id = 2, BookId = 2, BorrowerId = 2, LoanDate = new DateTime(2026, 1, 10), DueDate = new DateTime(2026, 2, 9), ReturnDate = (DateTime?)null },
            new { Id = 3, BookId = 3, BorrowerId = 3, LoanDate = new DateTime(2025, 12, 15), DueDate = new DateTime(2026, 1, 14), ReturnDate = (DateTime?)null },
            new { Id = 4, BookId = 4, BorrowerId = 4, LoanDate = new DateTime(2026, 1, 20), DueDate = new DateTime(2026, 2, 19), ReturnDate = (DateTime?)null },
            new { Id = 5, BookId = 5, BorrowerId = 1, LoanDate = new DateTime(2026, 1, 25), DueDate = new DateTime(2026, 2, 24), ReturnDate = (DateTime?)null },
            new { Id = 6, BookId = 6, BorrowerId = 2, LoanDate = new DateTime(2026, 2, 10), DueDate = new DateTime(2026, 3, 12), ReturnDate = (DateTime?)null },
            new { Id = 7, BookId = 7, BorrowerId = 3, LoanDate = new DateTime(2026, 2, 15), DueDate = new DateTime(2026, 3, 17), ReturnDate = (DateTime?)null },
            new { Id = 8, BookId = 8, BorrowerId = 4, LoanDate = new DateTime(2026, 2, 20), DueDate = new DateTime(2026, 3, 22), ReturnDate = (DateTime?)null },
            new { Id = 9, BookId = 9, BorrowerId = 1, LoanDate = new DateTime(2026, 2, 25), DueDate = new DateTime(2026, 3, 27), ReturnDate = (DateTime?)null },
            new { Id = 10, BookId = 10, BorrowerId = 2, LoanDate = new DateTime(2026, 3, 1), DueDate = new DateTime(2026, 3, 31), ReturnDate = (DateTime?)null },
            new { Id = 11, BookId = 1, BorrowerId = 3, LoanDate = new DateTime(2026, 1, 5), DueDate = new DateTime(2026, 2, 4), ReturnDate = new DateTime(2026, 1, 30) },
            new { Id = 12, BookId = 2, BorrowerId = 4, LoanDate = new DateTime(2026, 1, 7), DueDate = new DateTime(2026, 2, 6), ReturnDate = new DateTime(2026, 2, 5) },
            new { Id = 13, BookId = 3, BorrowerId = 1, LoanDate = new DateTime(2026, 1, 10), DueDate = new DateTime(2026, 2, 9), ReturnDate = new DateTime(2026, 2, 8) },
            new { Id = 14, BookId = 4, BorrowerId = 2, LoanDate = new DateTime(2026, 1, 15), DueDate = new DateTime(2026, 2, 14), ReturnDate = new DateTime(2026, 2, 10) },
            new { Id = 15, BookId = 5, BorrowerId = 3, LoanDate = new DateTime(2026, 1, 20), DueDate = new DateTime(2026, 2, 19), ReturnDate = new DateTime(2026, 2, 18) },
            new { Id = 16, BookId = 6, BorrowerId = 4, LoanDate = new DateTime(2026, 1, 25), DueDate = new DateTime(2026, 2, 24), ReturnDate = new DateTime(2026, 2, 20) },
            new { Id = 17, BookId = 7, BorrowerId = 1, LoanDate = new DateTime(2026, 1, 28), DueDate = new DateTime(2026, 2, 27), ReturnDate = new DateTime(2026, 2, 26) },
            new { Id = 18, BookId = 8, BorrowerId = 2, LoanDate = new DateTime(2026, 2, 1), DueDate = new DateTime(2026, 3, 3), ReturnDate = new DateTime(2026, 2, 25) },
            new { Id = 19, BookId = 9, BorrowerId = 3, LoanDate = new DateTime(2026, 2, 5), DueDate = new DateTime(2026, 3, 7), ReturnDate = new DateTime(2026, 2, 28) },
            new { Id = 20, BookId = 10, BorrowerId = 4, LoanDate = new DateTime(2026, 2, 8), DueDate = new DateTime(2026, 3, 10), ReturnDate = new DateTime(2026, 3, 1) },
            //Ekstra til mere end to udlån-query
            new { Id = 21, BookId = 1, BorrowerId = 2, LoanDate = new DateTime(2026, 3, 15), DueDate = new DateTime(2026, 4, 14), ReturnDate = (DateTime?)null }
        );
    }
}