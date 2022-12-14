namespace Farsica.Framework.Data
{
    using static Farsica.Framework.Core.Constants;

    public struct ResultData<T>
    {
        public ResultData(OperationResult operationResult)
        {
            OperationResult = operationResult;
        }

        public T? Data { get; set; }

        public OperationResult OperationResult { get; set; }

        public string? Error { get; set; }
    }
}
