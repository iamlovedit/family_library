using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServices.Infrastructure.Seed
{
    public class DatabaseContext
    {
        public DatabaseContext(ISqlSugarClient sqlSugarClient)
        {
            if (sqlSugarClient is SqlSugarScope scope)
            {
                _database = scope;
            }
        }

        private SqlSugarScope _database;
        public SqlSugarScope Database
        {
            get => _database;
            private set => _database = value;
        }
        private DbType _dbType;
        public DbType DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }
        public SimpleClient<T> GetEntityDB<T>() where T : class, new()
        {
            return new SimpleClient<T>(_database);
        }
    }
}