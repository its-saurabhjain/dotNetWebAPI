using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using OAuth.Data.Models;

namespace OAuth.Data.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository(Func<IDbConnection> openConnection) : base(openConnection) {}

        public async Task<User> GetAsync(string username, string password)
        {
            using (var connection = OpenConnection())
            {
                var queryResult = await connection.QueryAsync<User>("select * from [Users] where [Username]=@username and [Password]=@password", 
                    new { username, password });

                return queryResult.SingleOrDefault();
            }
        }
        public async Task<User> GetAsync(string username)
        {
            using (var connection = OpenConnection())
            {
                var queryResult = await connection.QueryAsync<User>("select * from [Users] where [Username]=@username",
                    new { username });

                return queryResult.SingleOrDefault();
            }
        }

        public async Task<IEnumerable<FriendRelation>> GetFriendsForAsync(User user)
        {
            using (var connection = OpenConnection())
            {
                return await connection.QueryAsync<FriendRelation>("select * from [Friends] where [InitiaterId]=@userId or [FriendId]=@userId", 
                    new { userId = user.Id });
            }
        }
        public void RegisterUser(string username, string password) {

            using (var ctx = OpenConnection())
            {
                string insertQuery = @"INSERT INTO [dbo].[Users]([Id],[Username], [Password], [CreatedOn]) 
                                        VALUES (@Id, @username, @password, @CreatedOn)";

                var result = ctx.Execute(insertQuery, new
                {
                    Id = new Guid(),
                    username,
                    password,
                    CreatedOn = DateTime.Now
                });
            }
        }
    }

    public interface IUserRepository
    {
        Task<IEnumerable<FriendRelation>> GetFriendsForAsync(User user);
        Task<User> GetAsync(string username, string password);
        Task<User> GetAsync(string username);
        void RegisterUser(string username, string password);
    }
}