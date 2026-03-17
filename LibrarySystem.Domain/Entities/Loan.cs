using System;

namespace LibrarySystem.Domain.Entities
{
    public class Loan
    {
        public int Id { get; private set; }

        public int BookId { get; private set; }//FK
        public int BorrowerId { get; private set; }//FK
        public Book Book { get; private set; } = null!;//Navigation property. Giver EF forståelse for relationer. Bruges til at lave joins automatisk ved Include. Herefter kan man navigere mellem objekter.
        public Borrower Borrower { get; private set; } = null!;//Navigation property
        public DateTime LoanDate { get; private set; }//Udlåningsdato
        public DateTime DueDate { get; private set; }//Dato afleveringsfrist
        public DateTime? ReturnDate { get; private set; }//Dato for hvornår bogen blev afleveret

        public bool IsReturned => ReturnDate != null;//Derived property, afledt af andre værdier. Tjekker om bogen er afleveret

        private Loan() { }//EF-Core opretter selv objekt via new, når den henter fra DB


        private Loan(int bookId, int borrowerId, DateTime loanDate, DateTime dueDate)//Private constructor til Factory Method, som sikrer, at objektet er i valid state, ellers kan det ikke oprettes
        {
            BookId = bookId;
            BorrowerId = borrowerId;
            LoanDate = loanDate;
            DueDate = dueDate;
        }

        public static Loan Create(int bookId, int borrowerId)
        {
            if (bookId <= 0)
                throw new ArgumentOutOfRangeException(nameof(bookId), "BookId skal være > 0");
            if (borrowerId <= 0)
                throw new ArgumentOutOfRangeException(nameof(borrowerId), "BorrowerId skal være > 0");

            var loanDate = DateTime.Now;//Udlånsdato og tid er altid NU
            var dueDate = loanDate.AddDays(30);//Max. 30 dages låneperiode

            return new Loan(bookId, borrowerId, loanDate, dueDate);
        }


        //Business logic
        public void ReturnBook(DateTime? returnDate = null)//Return af bog fra vilkårlig dato (ved f.eks. efterslæb af indtastning) og ellers NU
        {
            if (IsReturned)
                throw new InvalidOperationException("Bogen er allerede afleveret");

            var actualReturnDate = returnDate ?? DateTime.Now;

            if (actualReturnDate < LoanDate)
                throw new ArgumentException("ReturnDate kan ikke være før LoanDate", nameof(returnDate));

            ReturnDate = actualReturnDate;
        }

        public bool IsOverdue(DateTime? referenceDate = null)//Tjekker om lånetiden er overskredet fra vilkårlig dato eller NU
        {
            var currentDate = referenceDate ?? DateTime.Now;
            return !IsReturned && currentDate > DueDate;
        }

        public int DaysOverdue(DateTime? referenceDate = null)//Hvor mange dage er bogen overskredet returnDate
        {
            var currentDate = referenceDate ?? DateTime.Now;
            if (!IsOverdue(currentDate)) return 0;
            return (currentDate.Date - DueDate.Date).Days;
        }
    }
}
