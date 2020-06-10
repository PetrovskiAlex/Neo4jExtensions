namespace Neo4jExtensions.Tests.Model
{
    [GraphElement(Name = "ROUTE")]
    public class Route
    {
        public int Order { get; set; }
        public RouteCondition Condition { get; set; }
    }
}