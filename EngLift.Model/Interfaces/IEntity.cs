namespace EngLift.Model.Interfaces
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
