﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Dapper;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using NUnit.Framework;

namespace DapperExtensions.Test.IntegrationTests.SqlServer
{
    public class SqlServerBaseFixture
    {
        protected IDatabase Db;

        [SetUp]
        public virtual void Setup()
        {
            var connection = new SqlConnection("Server=localhost;Database=dapperTest;User Id=dapperTest;Password=dapperTest!.1;");
            var config = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new SqlServerDialect());
            var sqlGenerator = new SqlGeneratorImpl(config);
            Db = new Database(connection, sqlGenerator);
            var files = new List<string>
                                {
                                    ReadScriptFile("CreateAnimalTable"),
                                    ReadScriptFile("CreateFooTable"),
                                    ReadScriptFile("CreateMultikeyTable"),
                                    ReadScriptFile("CreatePersonTable"),
                                    ReadScriptFile("CreateCarTable")
                                };

            foreach (var setupFile in files)
            {
                connection.Execute(setupFile);
            }
        }

        public string ReadScriptFile(string name)
        {
            string fileName = "IntegrationTests/SqlServer/Sql/" + name + ".sql";
            using (Stream s = File.OpenRead(fileName))
                using (StreamReader sr = new StreamReader(s))
                {
                    return sr.ReadToEnd();
                }
        }
    }
}