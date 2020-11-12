using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Application.Queries
{
    public interface IDbQueryHelper
    {
        public Task<IEnumerable<T>> Query<T>(string sql, object param = default);

        public Task<T> QuerySingle<T>(string sql, object param = default);
    }
}