using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicService.Services
{
    public interface IBaseService<T, T2>
    {
        Task<Result<int>> AddOrUpdate(T city);
        Task<Result<bool>> Remove(int id);
        Task<Result<Dictionary<int, T2>>> GetList(int[] idArr);
        Task<Result<T2>> Get(int id);
        Task<Result<bool>> RemoveAll(IEnumerable<int> exceptIds, int? vendorId = null);
    }
}
