namespace Moya.Models
{
    using System;

    public class TestCase
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public string FilePath { get; set; }
        public string MethodName { get; set; }
        public int LineNumber { get; set; }
        public string ClassName { get; set; }
    }
}