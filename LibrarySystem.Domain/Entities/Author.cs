using System;
using System.Collections.Generic;

namespace LibrarySystem.Domain.Entities
{
    public class Author
    {
        public int Id { get; private set; }//PK, sættes automatisk ved SaveChanges()

        public string FirstName { get; private set; }
        public string LastName { get; private set; } //FullName laves ved Select i query
      
        public string Nationality { get; private set; }
        public int BirthYear { get; private set; }

        private List<Book> _books = new();//Private. Beskytter encapsulation

        public IReadOnlyCollection<Book> Books => _books;//Read-only, se, men ikke røre!

        private Author() { }//EF-Core obretter selv objekt via new, når den henter fra DB, constructoren skal derfor være tom ellers fejler EF

        public Author(string firstName, string lastName, string nationality, int birthYear)//Constructor sikrer valid state ved oprettelse
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("FirstName required", nameof(firstName));//Guard clauses
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("LastName required", nameof(lastName));
            if (string.IsNullOrWhiteSpace(nationality)) throw new ArgumentException("Nationality required", nameof(nationality));
            if (birthYear < 0 || birthYear > DateTime.Now.Year) throw new ArgumentOutOfRangeException(nameof(birthYear));

            FirstName = firstName;
            LastName = lastName;
            Nationality = nationality;
            BirthYear = birthYear;
        }
    }
}