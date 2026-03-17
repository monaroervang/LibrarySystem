using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Console.DTOs;

public record LoanDetailsDto(
    int LoanId,
    string BorrowerName,
    string BookTitle,
    string AuthorName,
    DateTime LoanDate,
    DateTime DueDate
);