using System.Collections.Generic;
using System.Data.SqlClient;
using Scaffold.Configuration;
using Scaffold.Exceptions;

namespace Scaffold.Generator.Database
{
    public class DatabaseGenerator : BaseGenerator
    {
        private readonly IConfiguration configuration;
        private static string command = "create-database";
        private static string description = "\tusage: create-database [sqlserver]\n\tCreates a new database";

        public DatabaseGenerator(IConfiguration configuration) : base(command, description)
        {
            this.configuration = configuration;
        }

        public override void Execute(List<string> args)
        {
            string sqlServer = ValidateAndReadArgs(args);

            var projectName = configuration.ProjectName;

            var connectionString = "data source=" + sqlServer + ";Initial Catalog=Master;Integrated Security=SSPI;";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string createDatabaseQuery = "CREATE DATABASE " + projectName.Replace(".", "");
                var createDatabaseCommand = new SqlCommand(createDatabaseQuery, connection);
                createDatabaseCommand.ExecuteNonQuery();
            }
        }

        private static string ValidateAndReadArgs(List<string> args)
        {
            if (args.Count == 1)
            {
                return args[0];
            }
            if (args.Count == 0)
            {
                return "localhost\\SQLEXPRESS";
            }
            throw new IllegalGeneratorArgs(description);
        }
    }
}