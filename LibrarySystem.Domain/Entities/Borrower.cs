using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Domain.Entities
{
    public class Borrower
    {
        public int Id { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }//FullName laves ved Select i query
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Address { get; private set; }

        private List<Loan> _loans = new();

        public IReadOnlyCollection<Loan> Loans => _loans;

        private Borrower() { }

        public Borrower(string firstName, string lastName, string email, string phone, string address)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("FirstName required", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("LastName required", nameof(lastName));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email required", nameof(email));
            if (!email.Contains("@")) throw new ArgumentException("Email er ikke gyldig", nameof(email));
            if (string.IsNullOrWhiteSpace(phone)) throw new ArgumentException("Phone required", nameof(phone));
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Address required", nameof(address));

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Address = address;
        }

        //Business logic
        public bool CanBorrow()//Max 5 aktive lån
        {
            int activeLoans = _loans.Count(l => !l.IsReturned);//Tæller aktive lån (ikke afleverede). True hvis < 5
            return activeLoans < 5;
        }

        public int ActiveLoanCount()//Lånerens antal aktive lån. Er f.eks. nyttig til statistik og rapporter
        {
            return _loans.Count(l => !l.IsReturned);
        }
    }
}
