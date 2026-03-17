using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Console.DTOs;

public record LoanOverviewDto(
    int LoanId,
    string BorrowerName,
    string BookTitle,
    DateTime LoanDate
);
