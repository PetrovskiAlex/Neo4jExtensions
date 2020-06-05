using System;

namespace Neo4jExtensions
{
    public class GraphElementAttribute : Attribute
    {
        public string Name { get; set; }
    }
}