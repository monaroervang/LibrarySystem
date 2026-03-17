using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Console.DTOs;

public record BookSearchDto(
    int Id,
    string Title,
    int PublicationYear
);
