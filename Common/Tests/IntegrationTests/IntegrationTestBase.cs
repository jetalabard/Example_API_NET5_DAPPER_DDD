using Common.Infrastructure.Repository;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection;

namespace Common.Tests.IntegrationTests
{
    public abstract class IntegrationTestBase<TContext> where TContext : DbContext
    {
        protected readonly IDbConnection Dbconnection;
        protected TContext ExampleContext;
        protected readonly DbContextOptionsBuilder<TContext> OptionsBuilder;
        private readonly string _databaseName;

        protected IntegrationTestBase()
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
            SqlMapper.AddTypeHandler(new GuidHandler());
            SqlMapper.AddTypeHandler(new TimeSpanHandler());

            _databaseName = GenerateNameDatabase();
            string connectionString = $"Data Source ={_databaseName}.db;Foreign Keys=False";
            Dbconnection = new SqliteConnection(connectionString);
            Dbconnection.Open();

            OptionsBuilder = new DbContextOptionsBuilder<TContext>();
            OptionsBuilder.UseSqlite((DbConnection)Dbconnection);
            OptionsBuilder.ReplaceService<IValueConverterSelector, CustomConverterSelector>();

            ExampleContext = (TContext)Activator.CreateInstance(typeof(TContext), new object[] { OptionsBuilder.Options });
            ExampleContext.Database.Migrate();
        }

        private static string GenerateNameDatabase()
        {
            return $"Test_Example_{Guid.NewGuid()}";
        }

        public virtual void DeleteDatabase()
        {
            try
            {
                var directory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(IntegrationTestBase<TContext>)).Location);
                string fileToRemove = Path.Combine(directory, _databaseName+".db");
                if (File.Exists(fileToRemove))
                {
                    File.Delete(fileToRemove);
                }
            }
            catch (Exception k)
            {
                Console.WriteLine(k);
            }
        }
    }
}
