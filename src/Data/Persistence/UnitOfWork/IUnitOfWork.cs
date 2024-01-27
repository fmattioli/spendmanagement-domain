using Domain.Interfaces;

namespace Data.Persistence.Interfaces
{
    public interface IUnitOfWork
    {
        ISpendManagementCommandRepository SpendManagementCommandRepository { get; }

        void Commit();
        void Dispose();
    }
}
