namespace Tools
{
    public interface IDotnetEF
    {
        string GenerateFromDatabase(string connectionString, DotnetEFProviders provider);
        string CreateMigration( string name);
        string CreateMigration(string context, string name);
        string UpdateDatabase(  );
        string UpdateDatabase(string context, string name);
    }
}