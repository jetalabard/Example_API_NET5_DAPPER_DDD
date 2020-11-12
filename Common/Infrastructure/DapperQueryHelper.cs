using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Common.Application.Queries;
using Dapper;

namespace Common.Infrastructure
{
    public class DapperQueryHelper : IDbQueryHelper
    {
        private readonly Func<IDbConnection> _connectionFactory;

        public DapperQueryHelper(Func<IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<T>> Query<T>(string sql, object param = default)
        {
            using var connection = _connectionFactory();
            return await connection.QueryAsync<T>(sql, param);
        }

        public async Task<T> QuerySingle<T>(string sql, object param = default)
        {
            using var connection = _connectionFactory();
            return await connection.QuerySingleOrDefaultAsync<T>(sql, param);
        }
    }
}
