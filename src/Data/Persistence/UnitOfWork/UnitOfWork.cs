using Data.Persistence.Interfaces;
using Domain.Interfaces;
using System.Data;

namespace Data.Persistence.UnitOfWork
{
    public class UnitOfWork(IDbTransaction dbTransaction,
        ISpendManagementCommandRepository spendManagementCommandRepository) : IUnitOfWork, IDisposable
    {
        public ISpendManagementCommandRepository SpendManagementCommandRepository { get; } = spendManagementCommandRepository;

        private readonly IDbTransaction _dbTransaction = dbTransaction;

        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
                _dbTransaction.Connection?.BeginTransaction();
            }
            catch (Exception)
            {
                _dbTransaction.Rollback();
            }
        }

        public void Dispose()
        {
            _dbTransaction.Connection?.Close();
            _dbTransaction.Connection?.Dispose();
            _dbTransaction.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
