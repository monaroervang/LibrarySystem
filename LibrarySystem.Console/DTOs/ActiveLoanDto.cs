using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Console.DTOs;

public record ActiveLoanDto( //Bruger record i stedet for class, da jeg kun har brug for at oprette properties og constructor. Derudover kan objektet ikke ændres efter new (immutable)
    int LoanId,
    string BorrowerName,
    string BookTitle,
    DateTime LoanDate,
    DateTime DueDate
);
