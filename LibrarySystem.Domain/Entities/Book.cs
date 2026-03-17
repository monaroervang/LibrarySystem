using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Domain.Entities
{
    public class Book
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public int PublicationYear { get; private set; }
        public int Pages { get; private set; }
        public string Genre { get; private set; }
        public string Isbn { get; private set; }

        public int AuthorId { get; private set; }//FK fra Author

        public Author Author { get; private set; } = null!;//Navigation property (1-M relation: Èn Author kan have mange Books)

        private List<Loan> _loans = new();

        public IReadOnlyCollection<Loan> Loans => _loans;

        private Book() { }

        public Book(string title, int publicationYear, int pages, string genre, string isbn, int authorId)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title required", nameof(title));
            if (publicationYear < 0 || publicationYear > DateTime.Now.Year) throw new ArgumentOutOfRangeException(nameof(publicationYear));
            if (pages <= 0) throw new ArgumentOutOfRangeException(nameof(pages), "Pages skal være > 0");
            if (string.IsNullOrWhiteSpace(genre)) throw new ArgumentException("Genre required", nameof(genre));
            if (string.IsNullOrWhiteSpace(isbn)) throw new ArgumentException("Isbn required", nameof(isbn));
            if (authorId <= 0) throw new ArgumentOutOfRangeException(nameof(authorId), "AuthorId skal være > 0");

            Title = title;
            PublicationYear = publicationYear;
            Pages = pages;
            Genre = genre;
            Isbn = isbn;
            AuthorId = authorId;
        }

        //Business logic
        public bool IsAvailable()//Max én udlåner ad gangen. Tager ét lån ad gangen og tjekker, om det er udlånt/ikke afleveret. True hvis ingen lån/alle lån er afleveret, False ved aktivt lån.
        {
            return !_loans.Any(l => !l.IsReturned);
        }
    }
}