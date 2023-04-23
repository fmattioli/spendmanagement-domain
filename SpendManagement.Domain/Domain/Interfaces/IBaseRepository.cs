namespace Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<Guid> Add(T entity, string sqlCommand);
    }
}
