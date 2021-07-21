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
        private readonly string _pgVersion;
        private readonly NpgsqlConnectionStringBuilder _connectionStringBuilder;

        private readonly Func<string, string, string> ReadEnvironmentVariable = (string keyName, string @default)
            => Environment.GetEnvironmentVariable(keyName) ?? @default;

        public AppPostgreSQLDbContext()
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

        public AppPostgreSQLDbContext(string pgHost, string pgPort, string pgDatabase, string pgUser, string pgPassword, string pgVersion)
        {
            // This constructor is used when the application is run. Parameters are injected by DI system.
            _connectionStringBuilder = new()
            {
                Host = pgHost ?? string.Empty,
                Port = int.Parse(pgPort ?? "5432"),
                Database = pgDatabase ?? string.Empty,
                Username = pgUser ?? string.Empty,
                Password = pgPassword ?? string.Empty,
                PersistSecurityInfo = false,
                TrustServerCertificate = true,
                Pooling = true
            };
            _pgVersion = pgVersion ?? "11.8";
        }

        public AppPostgreSQLDbContext(DbContextOptions<AppDbContext> dbContextOptions)
            : base(dbContextOptions)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionStringBuilder != null)
                optionsBuilder.UseNpgsql(_connectionStringBuilder.ConnectionString, options => options.EnableRetryOnFailure(3).SetPostgresVersion(new Version(_pgVersion)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _ = new AuthorMap(modelBuilder.Entity<Author>());
            _ = new BookMap(modelBuilder.Entity<Book>());
        }
    }
}