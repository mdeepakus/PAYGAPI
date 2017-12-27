using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Filters;
using PAYG.Infrastructure.Repository;
using System;
using System.Threading.Tasks;

namespace PAYG.Server.Infrastructure.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class DbTransactionFilter : IAsyncActionFilter
    {
        private readonly ITransactionManager _transactionManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionManager"></param>
        public DbTransactionFilter(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                _transactionManager.BeginTransaction();
                await next();
                _transactionManager.CommitTransaction();
            }
            catch (Exception ex)
            {
                _transactionManager.RollbackTransaction();
                throw ex;
            }
        }
    }
}
