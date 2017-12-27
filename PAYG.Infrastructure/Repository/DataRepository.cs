using Dapper;
using PAYG.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PAYG.Infrastructure.Repository
{
    /// <summary>
    /// A class which implements low-level calls to the database on behalf of repository classes.
    /// </summary>
    public class DataRepository : IDataRepository
    {
        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRepository"/> class.
        /// </summary>
        /// <param name="dbTransaction">A transaction object within which all data operations will take place.</param>
        public DataRepository(IDbTransaction dbTransaction)
        {
            DbTransaction = dbTransaction;
            DbConnection = DbTransaction.Connection;
        }

        #endregion ctor

        #region Public Members

        /// <inheritdoc cref="IDataRepository"/>
        public IDbConnection DbConnection { get; }

        /// <inheritdoc cref="IDataRepository"/>
        public IDbTransaction DbTransaction { get; }

        /// <inheritdoc cref="IDataRepository"/>
        public async Task<IEnumerable<dynamic>> QueryAsync(string select, object parameters = null, CommandType? commandType = null)
        {
            return await DbConnection.QueryAsync(select, parameters, transaction: DbTransaction, commandType: commandType);
        }

        /// <inheritdoc cref="IDataRepository"/>
        public async Task<IEnumerable<T>> QueryAsync<T>(string select, object parameters = null, CommandType? commandType = null)
        {
            return await DbConnection.QueryAsync<T>(select, parameters, transaction: DbTransaction, commandType: commandType);
        }

        /// <inheritdoc cref="IDataRepository"/>
        public IEnumerable<dynamic> Query(string select, object parameters = null, CommandType? commandType = null)
        {
            return DbConnection.Query(select, parameters, transaction: DbTransaction, commandType: commandType);
        }

        /// <inheritdoc cref="IDataRepository"/>
        public IEnumerable<T> Query<T>(string select, object parameters = null, CommandType? commandType = null)
        {
            return DbConnection.Query<T>(select, parameters, transaction: DbTransaction, commandType: commandType);
        }

        /// <inheritdoc cref="IDataRepository"/>
        public async Task<int> ExecuteAsync(string sql, object parameters = null, CommandType? commandType = null)
        {
            return await DbConnection.ExecuteAsync(sql, parameters, transaction: DbTransaction, commandType: commandType);
        }

        /// <inheritdoc cref="IDataRepository"/>
        public async Task<IEnumerable<dynamic>> PagedQueryAsync(string selectWhere, string orderBy, object parameters, int pageNumber, int pageSize)
        {
            var sql = GetPagedSql(selectWhere, orderBy, pageNumber, pageSize);

            return await DbConnection.QueryAsync(sql, parameters, transaction: DbTransaction);
        }

        /// <inheritdoc cref="IDataRepository"/>
        public async Task<IEnumerable<T>> PagedQueryAsync<T>(string selectWhere, string orderBy, object parameters, int pageNumber, int pageSize)
        {
            var sql = GetPagedSql(selectWhere, orderBy, pageNumber, pageSize);

            return await DbConnection.QueryAsync<T>(sql, parameters, transaction: DbTransaction);
        }

        /// <inheritdoc cref="IDataRepository"/>
        public async Task<T> ExecuteScalar<T>(string sql, object parameters = null, CommandType? commandType = null)
        {
            return await DbConnection.ExecuteScalarAsync<T>(sql, parameters, transaction: DbTransaction, commandType: commandType);
        }

        public async Task<List<dynamic>> ExecuteStoredProcedure(string storedProcedure, Dictionary<string, string> parameters = null)
        {
            Ensure.ArgumentNotNull(storedProcedure, nameof(storedProcedure));

            DynamicParameters param = null;
            if (parameters != null)
            {
                param = new DynamicParameters(parameters.Where(p => p.Key != "StoredProcedure").ToDictionary(p => p.Key, p => (object)p.Value));
            }

            var result = await QueryAsync(storedProcedure, param, commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return await DbConnection.QueryAsync<TFirst, TSecond, TReturn>(sql, map, param, DbTransaction, buffered, splitOn, commandTimeout, commandType);
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return await DbConnection.QueryAsync<TFirst, TSecond, TThird, TReturn>(sql, map, param, DbTransaction, buffered, splitOn, commandTimeout, commandType);
        }

        #endregion Public Members

        #region Private Members

        private static string GetPagedSql(string selectWhere, string orderBy, int pageNumber, int pageSize)
        {
            selectWhere = selectWhere.Replace("\r\n", string.Empty).Trim();

            var sql = @"
                WITH CTE AS
                    (
                      SELECT
                        ROW_NUMBER() OVER ( {1} ) AS RowNum,
                        {0}
                    )
                    SELECT *, (select count(0) from CTE) AS TotalCount FROM CTE
                    WHERE RowNum > {2}
                      AND RowNum <= {3}
                    Order By RowNum
            ";

            if (selectWhere.StartsWith("SELECT ", StringComparison.OrdinalIgnoreCase))
            {
                selectWhere = selectWhere.Substring(6);
            }

            sql = string.Format(sql, selectWhere, orderBy, (pageNumber - 1) * pageSize, pageNumber * pageSize);
            return sql;
        }

        #endregion Private Members
    }
}
