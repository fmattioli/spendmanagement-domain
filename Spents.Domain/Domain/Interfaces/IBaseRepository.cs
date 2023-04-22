namespace Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<int> Add(T entity, string table);
        Task<bool> Delete(Guid Id);
        Task<bool> Update(Guid Id);
    }
}
