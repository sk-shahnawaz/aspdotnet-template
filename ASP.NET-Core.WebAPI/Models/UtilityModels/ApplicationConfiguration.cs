namespace ASP.NET.Core.WebAPI.Models.UtilityModels
{
    public class ApplicationConfiguration
    {
        public string USE_IN_MEMORY_DATABASE { get; set; } = "false";
        public string IN_MEMORY_DATABASE_NAME { get; set; }
        public string ENABLE_ODATA { get; set; } = "false";
        public string USE_POSTGRESQL_DB { get; set; } = "true";
        public string USE_SQLSERVER_DB { get; set; } = "false";

        internal bool UseInMemoryDatabase
        {
            get => string.Equals(USE_IN_MEMORY_DATABASE, "true", System.StringComparison.OrdinalIgnoreCase);
        }

        internal bool EnableOData
        {
            get => string.Equals(ENABLE_ODATA, "true", System.StringComparison.OrdinalIgnoreCase);
        }
        internal bool UsePostgreSql
        {
            get => string.Equals(USE_POSTGRESQL_DB, "true", System.StringComparison.OrdinalIgnoreCase);
        }
        internal bool UseSqlServer
        {
            get => string.Equals(USE_SQLSERVER_DB, "true", System.StringComparison.OrdinalIgnoreCase);
        }

    }
}