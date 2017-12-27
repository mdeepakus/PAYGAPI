using PAYG.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PAYG.Infrastructure.Repository
{
    public class TransactionManager : ITransactionManager
    {
        public TransactionManager(IDbConnection dbConnection)
        {
            Ensure.ArgumentNotNull(dbConnection, nameof(dbConnection));

            DbConnection = dbConnection;
        }

        private IDbConnection DbConnection { get; }

        private IDbTransaction CurrentTransaction { get; set; }

        public IDbTransaction CreateTransaction()
        {
            BeginTransaction();
            return CurrentTransaction;
        }

        public void BeginTransaction()
        {
            if (CurrentTransaction != null)
            {
                return;
            }

            if (DbConnection.State == ConnectionState.Closed)
            {
                DbConnection.Open();
            }

            CurrentTransaction = DbConnection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void CommitTransaction()
        {
            try
            {
                CurrentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (CurrentTransaction != null)
                {
                    CurrentTransaction.Dispose();
                    CurrentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                CurrentTransaction?.Rollback();
            }
            finally
            {
                if (CurrentTransaction != null)
                {
                    CurrentTransaction.Dispose();
                    CurrentTransaction = null;
                }
            }
        }
    }
}
