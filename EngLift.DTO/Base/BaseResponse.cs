namespace EngLift.DTO.Base
{
    public class SingleId
    {
        public Guid Id { get; set; }
    }
    public class SingleId<T>
    {
        public T Id { get; set; }
    }
}
