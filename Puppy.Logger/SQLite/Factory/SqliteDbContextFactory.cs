﻿#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Puppy </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Puppy </Project>
//     <File>
//         <Name> SqliteDbContextFactory.cs </Name>
//         <Created> 23/08/17 10:10:06 AM </Created>
//         <Key> b48e746c-d7ba-4574-b1c6-11809cdd98e1 </Key>
//     </File>
//     <Summary>
//         SqliteDbContextFactory.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Puppy.Core.StringUtils;
using Puppy.Core.TypeUtils;
using System.Reflection;

namespace Puppy.Logger.SQLite.Factory
{
    public class SqliteDbContextFactory : IDesignTimeDbContextFactory<DbContext>
    {
        public static string GetConnectionString()
        {
            return new SqliteConnectionStringBuilder
            {
                DataSource = LoggerConfig.SQLiteFilePath.GetFullPath(),
                Mode = SqliteOpenMode.ReadWriteCreate
            }.ConnectionString;
        }

        public static Assembly GetMigrationAssembly()
        {
            return typeof(ISqliteDatabase).GetAssembly();
        }

        public static string GetMigrationAssemblyName()
        {
            return typeof(ISqliteDatabase).GetAssemblySimpleName();
        }

        public DbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SqliteDbContext>();

            builder.UseSqlite(GetConnectionString(), optionsBuilder => optionsBuilder.MigrationsAssembly(GetMigrationAssemblyName()));

            return new SqliteDbContext(builder.Options);
        }
    }
}