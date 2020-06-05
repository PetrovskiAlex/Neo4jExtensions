using System;

namespace Neo4jExtensions.Tests.Model
{
    public class Tariff
    {
        public Guid Id { get; set; }
        public Kind Type { get; set; }
        public string Number { get; set; }
        public long Version { get; set; }
    }
}