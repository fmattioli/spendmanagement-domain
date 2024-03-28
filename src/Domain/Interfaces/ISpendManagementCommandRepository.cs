using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ISpendManagementCommandRepository : IBaseEventSourcingRepository<SpendManagementCommand>
    {
    }
}
