using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Models.Results
{
    public class DataResult<T> : Result
    {
        public T Data { get; set; }
    }
}
