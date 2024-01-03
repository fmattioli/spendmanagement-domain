using Domain.Interfaces;

namespace Data.Persistence.Interfaces
{
    public interface IUnitOfWork
    {
        ISpendManagementCommandRepository SpendManagementCommandRepository { get; }
        
        ISpendManagementEventRepository SpendManagementEventRepository { get; }

        void Commit();
        void Dispose();
    }
}
