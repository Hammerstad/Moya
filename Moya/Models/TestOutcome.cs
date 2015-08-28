namespace Moya.Models
{
    /// <summary>
    /// Represents the outcome of a Moya test, e.g. <see cref="F:TestOutcome.Success"/>.
    /// </summary>
    public enum TestOutcome
    {
        Failure,
        Ignored,
        Success,
        NotFound
    }
}