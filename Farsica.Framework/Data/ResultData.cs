namespace Farsica.Framework.Data
{
    using static Farsica.Framework.Core.Constants;

    public class ResultData<T>
    {
        public T? Data { get; set; }

        public OperationResult OperationResult { get; set; }

        public string? Error { get; set; }
    }
}
