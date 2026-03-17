using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Console.DTOs;

public record AuthorBookCountDto(
    int AuthorId,
    string AuthorName,
    int BookCount
);
