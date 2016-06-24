using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Results
{
    public class ValidationResult
    {
        public ValidationResult() { }
        public ValidationResult(string error) { }

        public string ErrorMessage { get; set; }

        public bool Valid => ErrorMessage != null;

        public static implicit operator bool(ValidationResult result)
        {
            return result.Valid;
        }
    }

    public class ValidationResult<TFailure> where TFailure : struct
    {
        public ValidationResult() { }
        public ValidationResult(TFailure failure) : this(failure, failure.ToString()) { }
        public ValidationResult(TFailure failure, string error)
        {
            Failure = failure;
            ErrorMessage = error;
        }

        public TFailure? Failure { get; private set; }

        public string ErrorMessage { get; private set; }

        public bool Valid => !Failure.HasValue;

        public static implicit operator bool(ValidationResult<TFailure> result)
        {
            return result.Valid;
        }

        public static implicit operator TFailure?(ValidationResult<TFailure> result)
        {
            return result.Failure;
        }
    }
}
