using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PAYG.Infrastructure.Repository
{
    /// <summary>
    /// An inteface which must be implemented by any class which provides low-level access to a database for repository classes.
    /// </summary>
    public interface IDataRepository
    {
        /// <summary>
        /// A valid and opened database connection.
        /// </summary>
        IDbConnection DbConnection { get; }

        /// <summary>
        /// A valid transaction scope.
        /// </summary>
        IDbTransaction DbTransaction { get; }

        /// <summary>
        /// Execute a query asynchronously.
        /// </summary>
        /// <param name="select">The query to execute.</param>
        /// <param name="parameters">One or more parameters required for the execution of the query.</param>
        /// <param name="commandType">The command type of the query.</param>
        /// <returns>A task of type an enumerable collection of dynamic objects.</returns>
        Task<IEnumerable<dynamic>> QueryAsync(string select, object parameters = null, CommandType? commandType = null);

        /// <summary>
        /// Execute a query asynchronously.
        /// </summary>
        /// <typeparam name="T">Type parameter specifying the collection type.</typeparam>
        /// <param name="select">The query to execute.</param>
        /// <param name="parameters">One or more parameters required for the execution of the query.</param>
        /// <param name="commandType">The command type of the query.</param>
        /// <returns>A task of type an enumerable collection of dynamic objects.</returns>
        Task<IEnumerable<T>> QueryAsync<T>(string select, object parameters = null, CommandType? commandType = null);

        /// <summary>
        /// Execute a query asynchronously.
        /// </summary>
        /// <param name="select">The query to execute.</param>
        /// <param name="parameters">One or more parameters required for the execution of the query.</param>
        /// <param name="commandType">The command type of the query.</param>
        /// <returns>A task of type an enumerable collection of dynamic objects.</returns>
        IEnumerable<dynamic> Query(string select, object parameters = null, CommandType? commandType = null);

        /// <summary>
        /// Execute a query asynchronously.
        /// </summary>
        /// <typeparam name="T">Type parameter specifying the collection type.</typeparam>
        /// <param name="select">The query to execute.</param>
        /// <param name="parameters">One or more parameters required for the execution of the query.</param>
        /// <param name="commandType">The command type of the query.</param>
        /// <returns>A task of type an enumerable collection of dynamic objects.</returns>
        IEnumerable<T> Query<T>(string select, object parameters = null, CommandType? commandType = null);

        /// <summary>
        /// Execute a query asynchronously.
        /// </summary>
        /// <param name="sql">The query to execute.</param>
        /// <param name="parameters">One or more parameters required for the execution of the query.</param>
        /// <param name="commandType">The command type of the query.</param>
        /// <returns>An enumerable collection of dynamic objects.</returns>
        Task<int> ExecuteAsync(string sql, object parameters = null, CommandType? commandType = null);

        /// <summary>
        /// Execute a query asynchronously.
        /// </summary>
        /// <param name="selectWhere">The query to execute.</param>
        /// <param name="orderBy">A sql clause specifying the order in which rows should be returned.</param>
        /// <param name="parameters">One or more parameters required for the execution of the query.</param>
        /// <param name="pageNumber">The starting page number when returned data is organised into pages.</param>
        /// <param name="pageSize">The size of each page when returned data is organised into pages.</param>
        /// <returns>A task of type an enumerable collection of dynamic objects.</returns>
        Task<IEnumerable<dynamic>> PagedQueryAsync(string selectWhere, string orderBy, object parameters, int pageNumber, int pageSize);

        /// <summary>
        /// Execute a query asynchronously.
        /// </summary>
        /// <typeparam name="T">Type parameter specifying the collection type.</typeparam>
        /// <param name="selectWhere">The query to execute.</param>
        /// <param name="orderBy">A sql clause specifying the order in which rows should be returned.</param>
        /// <param name="parameters">One or more parameters required for the execution of the query.</param>
        /// <param name="pageNumber">The starting page number when returned data is organised into pages.</param>
        /// <param name="pageSize">The size of each page when returned data is organised into pages.</param>
        /// <returns>A task of type an enumerable collection of dynamic objects.</returns>
        Task<IEnumerable<T>> PagedQueryAsync<T>(string selectWhere, string orderBy, object parameters, int pageNumber, int pageSize);

        /// <summary>
        /// Executes a sql command and returns the single value result
        /// </summary>
        /// <typeparam name="T">Type of return value</typeparam>
        /// <param name="sql">Sql statement</param>
        /// <param name="parameters">One or more parameters required to execute the SQL statement</param>
        /// <param name="commandType">The command type of the SQL</param>
        /// <returns>A Task containing the result of the action</returns>
        Task<T> ExecuteScalar<T>(string sql, object parameters, CommandType? commandType = null);

        /// <summary>
        /// Executes a stored procedure and retiurns the result as a list of dynamic
        /// </summary>
        /// <param name="storedProcedure">Stored procedure name</param>
        /// <param name="parameters">dictionary of parameters</param>
        /// <returns>List of dynamic</returns>
        Task<List<dynamic>> ExecuteStoredProcedure(string storedProcedure, Dictionary<string, string> parameters = null);

        // Summary:
        //     Maps a query to objects
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        // Summary:
        //     Maps a query to objects
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        // Summary:
        //     Perform a multi mapping query with 4 input parameters
        //Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        ////
        //// Summary:
        ////     Perform a multi mapping query with 5 input parameters
        //Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        ////
        //// Summary:
        ////     Perform a multi mapping query with 6 input parameters
        //Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        ////
        //// Summary:
        ////     Perform a multi mapping query with 7 input parameters
        //Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
    }
}
