namespace ASP.NET.Core.WebAPI.Models.UtilityModels;

public class ApplicationConfiguration
{
    public string USE_IN_MEMORY_DATABASE { get; init; } = "false";
    public string IN_MEMORY_DATABASE_NAME { get; init; }
    public string ENABLE_ODATA { get; init; } = "false";
    public string USE_POSTGRESQL_DB { get; init; } = "true";
    public string USE_SQLSERVER_DB { get; init; } = "false";

    internal bool UseInMemoryDatabase
    {
        get => string.Equals(USE_IN_MEMORY_DATABASE, "true", StringComparison.OrdinalIgnoreCase);
    }

    internal bool EnableOData
    {
        get => string.Equals(ENABLE_ODATA, "true", StringComparison.OrdinalIgnoreCase);
    }
    internal bool UsePostgreSql
    {
        get => string.Equals(USE_POSTGRESQL_DB, "true", StringComparison.OrdinalIgnoreCase);
    }
    internal bool UseSqlServer
    {
        get => string.Equals(USE_SQLSERVER_DB, "true", StringComparison.OrdinalIgnoreCase);
    }
}