using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class Result<T>
    {
        public Result()
        {
            ResultCode = 200;
        }

        public Result(T obj)
        {
            Data = obj;
            ResultCode = 200;
        }

        public T Data { get; set; }

        public int ResultCode { get; set; }

        public string Message { get; set; }
    }
}
