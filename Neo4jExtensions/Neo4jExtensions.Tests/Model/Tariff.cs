using System;

namespace Neo4jExtensions.Tests.Model
{
    [GraphElement(Name = "Tariff")]
    public class Tariff
    {
        public Guid Id { get; set; }
        public Kind Kind { get; set; }
        public string Number { get; set; }
        public long Version { get; set; }
    }
}