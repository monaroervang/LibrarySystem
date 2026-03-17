namespace LibrarySystem.Infrastructure;

public static class DatabaseConfiguration //Connection-string
{
    public const string ConnectionString =
        "Server=(localdb)\\mssqllocaldb;Database=LibrarySystemDb;Trusted_Connection=True;TrustServerCertificate=True";
}//Jeg har valgt MSSQL fremfor SQLite, da det er en server-baseret database og jeg vil gerne øve mig på realistisk .NET udvikling. Serverbaseret givre nedre queryoptimering samt kan håndtere større datamængder.
//Større datamængder vil jeg mene er relevant i et administrationssystem til et bibliotek.