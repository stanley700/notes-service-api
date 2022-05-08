using NotesService.Core.Data.Entities;
using System.Threading.Tasks;

namespace NotesService.Core.Services.IServices
{
    public interface IUserService
    {
        Task<bool> CreateUser(User user);
        Task<User> GetUser(string username);
        Task<User> GetUser(string username, string hashPassword);
        Task<bool> UpdateUser(User user);
    }
}
