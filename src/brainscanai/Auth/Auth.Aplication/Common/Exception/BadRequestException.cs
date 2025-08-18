namespace Auth.Aplication.Common.Exception
{
    public class BadRequestException : System.Exception
    {
        public BadRequestException() : base()
        {

        }

        public BadRequestException(string message) : base(message)
        {

        }

        public BadRequestException(string message, System.Exception exp) : base(message, exp)
        {

        }
    }
}
