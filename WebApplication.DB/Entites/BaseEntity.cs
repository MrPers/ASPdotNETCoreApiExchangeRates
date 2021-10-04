using WebApplication.DTO;

namespace WebApplication.DB.Entites
{
    public class BaseEntity<T>: IBaseEntity<T>
    {
        public T Id { get; set; }
    }
}
