namespace Farsica.Framework.Data
{
    using System.Collections.Generic;
    using static Farsica.Framework.Core.Constants;

    public struct ResultData<T>(OperationResult operationResult)
    {
        public T? Data { get; set; }

        public OperationResult OperationResult { get; set; } = operationResult;

        public IEnumerable<Error>? Errors { get; set; }
    }
}
