using System;
using Npgsql;

using Microsoft.EntityFrameworkCore;
using NET.Core.Library.Domain.DBModels;
using NET.Core.Library.Domain.Infrastructure;
using NET.Core.Console.DB.PostgreSQL.Models.Maps;

namespace NET.Core.Console.DB.PostgreSQL
{
    /// <summary>
    /// Application's Database Context type, specifically for PostgreSQL RDBMS.
    /// </summary>
    public sealed class AppPostgreSQLDbContext : AppDbContext
    {
        private string _pgVersion;
        private NpgsqlConnectionStringBuilder _connectionStringBuilder;

        private readonly Func<string, string, string> ReadEnvironmentVariable = (string keyName, string @default)
            => Environment.GetEnvironmentVariable(keyName) ?? @default;

        public AppPostgreSQLDbContext()
        {
            // This constructor is used when this project is run as a standalone project to perform database migrations.
            ConfigurePostgreSqlConnection();
        }

        public AppPostgreSQLDbContext(DbContextOptions<AppPostgreSQLDbContext> dbContextOptions)
            : base(ChangeOptionsType(dbContextOptions))
        {
            // This constructor is used when the application is run. Parameters are injected by DI system.
            ConfigurePostgreSqlConnection();
        }

        private void ConfigurePostgreSqlConnection()
        {
            // This constructor is used when this project is run as standalone for performing DB Migrations.
            _connectionStringBuilder = new()
            {
                Host = ReadEnvironmentVariable("PGHOST", string.Empty),
                Port = int.Parse(ReadEnvironmentVariable("PGPORT", "5432")),
                Database = ReadEnvironmentVariable("PGDATABASE", string.Empty),
                Username = ReadEnvironmentVariable("PGUSER", string.Empty),
                Password = ReadEnvironmentVariable("PGPASSWORD", string.Empty),
                PersistSecurityInfo = false,
                TrustServerCertificate = true,
                Pooling = true
            };
            _pgVersion = ReadEnvironmentVariable("PGVERSION", "11.8");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionStringBuilder != null)
                optionsBuilder.UseNpgsql(_connectionStringBuilder.ConnectionString, options => options.EnableRetryOnFailure(3).SetPostgresVersion(new Version(_pgVersion)));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _ = new AuthorMap(modelBuilder.Entity<Author>());
            _ = new BookMap(modelBuilder.Entity<Book>());
        }
    }
}