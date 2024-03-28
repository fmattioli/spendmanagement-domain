using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ISpendManagementEventRepository : IBaseEventSourcingRepository<SpendManagementEvent>
    {
    }
}
