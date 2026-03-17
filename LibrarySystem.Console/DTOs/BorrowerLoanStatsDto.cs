using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Console.DTOs;

public record BorrowerLoanStatsDto(
    int BorrowerId,
    string BorrowerName,
    int ActiveLoans,
    int ReturnedLoans,
    int TotalLoans
);
