using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Console.DTOs;

public record BookSortedDto(//Bruger record i stedet for class, da jeg kun har brug for at oprette properties og constructor. Derudover kan objektet ikke ændres efter new (immutable)
    int Id,
    string Title,
    int PublicationYear
);