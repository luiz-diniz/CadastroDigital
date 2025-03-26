using CadastroDigital.Domain.Ports.Repository;

namespace CadastroDigital.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbSession _session;

        public UnitOfWork(DbSession session)
        {
            _session = session;
        }

        public void BeginTransaction()
        {
            _session.Transaction = _session.Connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _session.Transaction.Commit();
            Dispose();
        }

        public void RollbackTransaction()
        {
            _session.Transaction.Rollback();
            Dispose();
        }

        public void Dispose() => _session.Transaction?.Dispose();
    }
}
