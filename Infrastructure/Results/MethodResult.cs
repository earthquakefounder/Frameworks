using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Results
{
    public class MethodResult<TValue>
    {
        public MethodResult(TValue result)
        {
            Result = result;
        }

        public TValue Result { get; private set; }
    }
    //public class MethodResult
    //{
    //    public MethodResult() { }
    //    public MethodResult(string error)
    //    {
    //        ErrorMessage = error;
    //    }

    //    public string ErrorMessage { get; set; }

    //    public bool IsValid => ErrorMessage == null;
    //}

    //public class MethodResult<TFailure> where TFailure : struct
    //{
    //    public MethodResult() { }
    //    public MethodResult(TFailure failed) : this(failed, failed.ToString()) { }
    //    public MethodResult(TFailure failed, string message)
    //    {
    //        Failure = failed;
    //        ErrorMessage = message;
    //    }

    //    public TFailure? Failure { get; private set; }

    //    public string ErrorMessage { get; private set; }

    //    public bool IsValid => Failure.HasValue;
    //}
}
