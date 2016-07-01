using BrightstarDB.EntityFramework;

namespace BrightstarDB.ConcurrencyIssue
{
    [Entity("kp:entity")]
    public interface IEntity
    {
        [Identifier]
        string Id { get; }
    }

    [Entity("kp:simpleEntity")]
    public interface ISimpleEntity : IEntity
    {
        [PropertyType("kp:identifier")]
        string Identifier { get; set; }
    }

    [Entity("kp:complexEntity1")]
    public interface IComplexEntity1 : IEntity
    {
        [PropertyType("kp:counter")]
        int Counter { get; set; }
    }

    [Entity("kp:complexEntity2")]
    public interface IComplexEntity2 : IEntity
    {
        [PropertyType("kp:name")]
        string Name { get; set; }
    }
}
