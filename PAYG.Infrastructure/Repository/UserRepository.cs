using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using PAYG.Domain.Common;
using PAYG.Domain.Entities;
using PAYG.Domain.Extensions;
using PAYG.Domain.RepositoryInterfaces;
using PAYG.Infrastructure.Authentication;
using System.Threading.Tasks;
using System.Linq;

namespace PAYG.Infrastructure.Repository
{
   public class UserRepository :IUserRepository
    {
        private IDataRepository _dataRepository;
        public UserRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="hashedPassword"></param>
        /// <returns></returns>
        public async Task<int> CreateConsumerUser(string userName, string hashedPassword)
        {
            Ensure.ArgumentNotNull(userName, nameof(userName));
            Ensure.ArgumentNotNull(hashedPassword, nameof(hashedPassword));

            string strSQL = @"
                INSERT INTO PAYGUser
                    (
                    user_name,
                    password,
                    user_type,
                    is_deleted
                    )
                values
                    (
                    
                    @userName,
                    @hashedPassword,
                    'CONSUMER',
                    0
                    )
                SELECT @@IDENTITY;
                ";

            var id = await _dataRepository.DbConnection.QueryAsync<int>(strSQL, new { userName, hashedPassword },
                transaction: _dataRepository.DbTransaction);

            return id.SingleOrDefault();

        }

        public async Task<ApplicationUser> GetApplicationUser(int userId)
        {
            return await GetUserDetailsInternal(null, userId);
        }

        public async Task<ApplicationUser> GetApplicationUser(string userName)
        {
            Ensure.ArgumentNotNull(userName, nameof(userName));

            return await GetUserDetailsInternal(userName, 0);
        }

        public async Task<bool> UserExists(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Username must be supllied!!!");
            }

            var sql = @"
                SELECT user_id FROM PAYGUser 
                WHERE user_name = @UserName
                ";

            var query = await _dataRepository.QueryAsync<int>(sql, new { userName});

            int userId = query.SingleOrDefault<int>();

            return userId > 0;
        }

        private async Task<ApplicationUser> GetUserDetailsInternal(string username, int userid)
        {
            if ((string.IsNullOrEmpty(username) && userid == 0) ||
                (!string.IsNullOrEmpty(username) && userid > 0))
                {
                    throw new ArgumentException("Either of username or userid must be supllied!!!");
                }

            var sql = @"
                SELECT user_id, user_name, password
                FROM PAYGUser usr
                WHERE ISNULL(is_deleted, 0) = 0
            ";

            if (userid != 0)
            {
                sql += " AND user_id = @UserId";
            }
            else
            {
                sql += " AND user_name = @Username";
            }

            var result = await _dataRepository.QueryAsync(sql, new { UserId = userid, Username = username });
            var data = result.SingleOrDefault();

            if (data == null)
            {
                return null;
            }

            var user = new ApplicationUser(
                data.user_id,
                data.user_name,
                data.password);

            return user;
        }
    }
}
