using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using NET.Core.Library.Domain.DBModels;
using NET.Core.Library.Domain.Infrastructure;
using NET.Core.Console.DB.SqlServer.Models.Maps;

namespace NET.Core.Console.DB.SqlServer
{
    /// <summary>
    /// Application's Database Context type, specifically for Microsoft SQL Server RDBMS.
    /// </summary>
    public sealed class AppSqlServerDbContext : AppDbContext
    {
        private readonly SqlConnectionStringBuilder _connectionStringBuilder;

        private readonly Func<string, string, string> ReadEnvironmentVariable = (string keyName, string @default)
            => Environment.GetEnvironmentVariable(keyName) ?? @default;

        public AppSqlServerDbContext()
        {
			// This constructor is used when this project is run as standalone for performing DB Migrations.
			_connectionStringBuilder = new()    // C# 9.0
			{
				DataSource = string.Concat(ReadEnvironmentVariable("SQLHOST", string.Empty), "," , ReadEnvironmentVariable("SQLPORTT", "1433")),
				InitialCatalog = ReadEnvironmentVariable("SQLDATABASE", string.Empty),
                UserID = ReadEnvironmentVariable("SQLUSER", string.Empty),
                Password = ReadEnvironmentVariable("SQLPASSWORD", string.Empty),
				PersistSecurityInfo = false,
				MultipleActiveResultSets = true,
				TrustServerCertificate = true,
			};
		}

        public AppSqlServerDbContext(string sqlHost, string sqlPort, string sqlDatabase, string sqlUser, string sqlPassword)
        {
            // This constructor is used when the application is run. Parameters are injected by DI system.
            _connectionStringBuilder = new()    // C# 9.0
            {
                DataSource = string.Concat(sqlHost, ",", sqlPort),
                InitialCatalog = sqlDatabase,
                UserID = sqlUser,
                Password = sqlPassword,
                PersistSecurityInfo = false,
                MultipleActiveResultSets = true,
                TrustServerCertificate = true
            };
        }

        public AppSqlServerDbContext(DbContextOptions<AppDbContext> dbContextOptions)
            : base(dbContextOptions)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionStringBuilder != null)
                optionsBuilder.UseSqlServer(_connectionStringBuilder.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _ = new AuthorMap(modelBuilder.Entity<Author>());
            _ = new BookMap(modelBuilder.Entity<Book>());
        }
    }
}