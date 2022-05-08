using Microsoft.EntityFrameworkCore;
using NotesService.Core.Data;
using NotesService.Core.Data.Entities;
using NotesService.Core.Exceptions;
using NotesService.Core.Services.IServices;
using System.Threading.Tasks;

namespace NotesService.Core.Services
{
    public class UserService : IUserService
    {
        private NotesServiceDbContext _dbContext;
        public UserService(NotesServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateUser(User user)
        {
            if (user == null)
            {
                throw new MessageException(SystemCodes.InvalidRequest, "User detail is required");
            }

            //Check if Username already exist
            var userObj = await GetUser(user.Username);
            if (userObj != null)
            {
                //User already exist
                throw new MessageException(SystemCodes.UsernameAlreadyExist, "Username already exist");
            }

            _dbContext.Users.Add(user);
            await SaveChanges();

            return true;
        }

        private async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> GetUser(string username)
        {
            if(string.IsNullOrEmpty(username))
            {
                return null;
            }

            var userObj = await _dbContext.Users.FirstOrDefaultAsync(user => user.Username == username);

            return userObj;
        }

        public async Task<User> GetUser(string username, string hashPassword)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(hashPassword))
            {
                //Exception
                throw new MessageException(SystemCodes.UsernameAndPasswordAreRequired, "Username and Password are required");
            }

            var userObj = await _dbContext.Users.FirstOrDefaultAsync(user => user.Username == username);

            return userObj;
        }

        public async Task<bool> UpdateUser(User user)
        {
            if(user == null)
            {
                throw new MessageException(SystemCodes.InvalidRequest, "User detail is required");
            }

            var userObj = await _dbContext.Users.FirstOrDefaultAsync(user => user.Username == user.Username);
            if(userObj == null)
            {
                throw new MessageException(SystemCodes.DataNotFound, "User not found");
            }

            userObj.IsActive = user.IsActive;
            userObj.FirstName = user.FirstName;
            userObj.LastName = user.LastName;
            userObj.RoleId = user.RoleId;

            _dbContext.Users.Update(userObj);
            await SaveChanges();

            return true;
        }
    }
}
