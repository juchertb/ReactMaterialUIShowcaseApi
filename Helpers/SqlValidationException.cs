namespace ReactMaterialUIShowcaseApi.Helpers
{
    public class SqlValidationException : Exception
    {
        public long? ErrorRecordId { get; }
        public SqlValidationException(string message, long? errorRecordId)
            : base(message)
        {
            ErrorRecordId = errorRecordId;
        }
    }
}
