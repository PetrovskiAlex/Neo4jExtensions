using System;

namespace Neo4jExtensions.Tests.Model
{
    [GraphElement(Name = "Location")]
    public class Location
    {
        public Guid Id { get; set; }
    }
}