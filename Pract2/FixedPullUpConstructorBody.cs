using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedPUCB
{
    public class Result
    {
        public bool IsSuccessful { get; set; }

        public Result() { }

        public Result(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }
    }

    public class Result<T> : Result where T : class
    {
        public T Data { get; }

        public Result(bool isSuccessful, T data = null) 
            : base(isSuccessful)
        {
            Data = data;
        }
    }
}
