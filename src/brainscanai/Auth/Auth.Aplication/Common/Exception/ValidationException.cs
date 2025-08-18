namespace Auth.Aplication.Common.Exception
{
    public class ValidationException : System.Exception
    {
        public IDictionary<string, string[]> Errors { get; }
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();

        }
        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures
                .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                .ToDictionary(f => f.Key, f => f.ToArray());
        }

        public ValidationException(IEnumerable<IdentityError> errors) : this()
        {
            Errors = errors
                .GroupBy(x => x.Code, x => x.Description)
                .ToDictionary(f => f.Key, f => f.ToArray());
        }
    }
}
