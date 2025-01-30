﻿using Dapper;
using MySqlConnector;
using System.Diagnostics.CodeAnalysis;

namespace User.Query.Infrastructure.Migrations;

[ExcludeFromCodeCoverage]
public class Database
{
    public static void CreateDatabase(string connectionWithDatabase, string databaseName)
    {
        using var myConnection = new MySqlConnection(connectionWithDatabase);

        var parameters = new DynamicParameters();
        parameters.Add("name", databaseName);

        var registry = myConnection.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @name", parameters);

        if (!registry.Any())
        {
            myConnection.Execute($"CREATE DATABASE {databaseName}");
        }
    }
}
