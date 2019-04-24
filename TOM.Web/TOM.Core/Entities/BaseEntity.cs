namespace TOM.Core.Entities
{
    public class BaseEntity<T> where T : class
    {
        public virtual int Id { get; set; }
    }

}
