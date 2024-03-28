namespace Domain.Interfaces
{
    public interface IBaseEventSourcingRepository<T> where T : class
    {
        Task<Guid> Add(T entity);
    }
}
