using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace PAYG.Infrastructure.Repository
{
    public interface ITransactionManager
    {
        void BeginTransaction();

        void CommitTransaction();

        IDbTransaction CreateTransaction();

        void RollbackTransaction();
    }
}
