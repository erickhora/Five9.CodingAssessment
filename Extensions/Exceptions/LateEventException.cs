namespace Five9.CodingAssessment.Extensions.Exceptions
{
    public class LateEventException : Exception
    {
        public LateEventException(string message) : base(message) { }
    }
}
