using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Console.DTOs;

public record LoansPerMonthDto(
    int Year,
    int Month,
    int LoanCount
    );