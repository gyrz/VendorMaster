using Contracts;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Cache
{
    public interface IRedisCache
    {
        Task<Result<T>> Get<T>(string key, int id, Func<Task<Result<T>>> func);
        Task<T> AddOrUpdateCache<T>(string key, int id, T data);
        Task<Result<bool>> Remove<T>(string key, int id, Func<Task<Result<bool>>> func);
        Task<Result<IEnumerable<T>>> GetListEveryNMinutes<T>(string key, int minutes, Func<Task<Result<Dictionary<int,T>>>> func);
        Task<Result<IEnumerable<T>>> GetListUntracked<T>(string key, Func<int[], Task<Result<Dictionary<int, T>>>> func);
    }
}
