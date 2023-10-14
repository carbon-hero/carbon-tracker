using NPoco;
using StackExchange.Profiling;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Core
{
    public class ProfiledDatabase : Database
    {
        public ProfiledDatabase(IDbConnection connection) : base((DbConnection)connection) { }
        public ProfiledDatabase(string connectionString, DatabaseType type, DbProviderFactory dbProviderFactory) :
            base(connectionString, type, dbProviderFactory)
        { }

        protected override void OnException(Exception e)
        {
            base.OnException(e);
            e.Data["LastSQL"] = this.LastSQL;
        }

        protected override DbConnection OnConnectionOpened(DbConnection connection)
        {
            return new StackExchange.Profiling.Data.ProfiledDbConnection(connection as DbConnection, MiniProfiler.Current);
        }

    }
}