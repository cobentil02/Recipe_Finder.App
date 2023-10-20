using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.Models;
using System.Linq;


namespace app.Services
{

    public class UserCredentialsDatabase
    {
        private readonly SQLiteAsyncConnection _connection;


        //(codemillmatt, 2023)
        public UserCredentialsDatabase()

        {


            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "recipe.db");
            _connection = new SQLiteAsyncConnection(dbPath);

            _connection.CreateTableAsync<UserCredentials>().Wait();
        }

        //store username in database
        public async Task<UserCredentials> GetUserCredentialsAsync(string username)
        {
            return await _connection.Table<UserCredentials>()
                                     .Where(x => x.Username == username)
                                     .FirstOrDefaultAsync();
        }
        //store email in database
        public async Task<UserCredentials> GetUserCredentialsByEmailAsync(string email)
        {
            return await _connection.Table<UserCredentials>()
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task SetUserCredentialsAsync(UserCredentials credentials)
        {
            var existingUser = await _connection.Table<UserCredentials>()
                                                .Where(x => x.Username == credentials.Username)
                                                .FirstOrDefaultAsync();
            if (existingUser != null)
            {
                existingUser.Password = credentials.Password;
                await _connection.UpdateAsync(existingUser);
            }
            else
            {
                await _connection.InsertAsync(credentials);
            }
        }
        public async Task ResetPasswordByUsernameAsync(string username, string newPassword)
        {
            var existingUser = await GetUserCredentialsAsync(username);
            if (existingUser != null)
            {
                existingUser.Password = newPassword;
                await _connection.UpdateAsync(existingUser);
            }
        }

        public async Task RemoveUserCredentialsAsync(string username)
        {
            await _connection.Table<UserCredentials>()
                             .Where(x => x.Username == username)
            .DeleteAsync();
        }



    }
}
