namespace Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<int> Add(T entity, string sqlCommand);
    }
}
