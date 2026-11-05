namespace LoginRegisterV2
{
    /// <summary>
    /// The outcome of something a service did. Success, or a message that is safe
    /// to show the user. Replaces the old "returns null on success, error text on
    /// failure" convention, which was easy to misread.
    /// </summary>
    public class OperationResult
    {
        public bool Success { get; private set; }
        public string Error { get; private set; }

        public static OperationResult Ok()
        {
            return new OperationResult { Success = true };
        }

        public static OperationResult Fail(string error)
        {
            return new OperationResult { Success = false, Error = error };
        }
    }
}
