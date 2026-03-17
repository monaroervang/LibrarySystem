using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Console.DTOs;

public record BookLoanCountDto(
    int BookId,
    string Title,
    int LoanCount
);