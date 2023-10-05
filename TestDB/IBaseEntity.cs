namespace TestTask.Abstractions
{
    public interface IBaseEntity
    {
       public int ID { get; set; }
    }
    public interface IBaseEntity<T>
    {
       public T ID { get; set; }
    }
}
