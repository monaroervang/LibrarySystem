using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Console.DTOs;

public record TopBookDto(
    int BookId,
    string Title,
    int LoanCount
    );