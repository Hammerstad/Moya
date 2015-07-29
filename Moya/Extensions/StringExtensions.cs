namespace Moya.Extensions
{
    /// <summary>
    /// Extends the <see cref="string"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Formats the <see cref="string"/> value using the given <paramref name="arguments"/>. 
        /// </summary>
        /// <param name="value">The target <see cref="string"/> value.</param>
        /// <param name="arguments">The arguments used to format the <paramref name="value"/>.</param>
        /// <returns>The formatted <see cref="string"/> value.</returns>
        public static string FormatWith(this string value, params object[] arguments)
        {
            return string.Format(value, arguments);
        }
    }
}
