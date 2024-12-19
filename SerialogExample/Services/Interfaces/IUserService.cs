using SerialogExample.Models;

namespace SerialogExample.Services.Interfaces;

public interface IUserService
{
    Task<User> CreateUser(User user);
    Task<IEnumerable<User>> GetAllUsers();
}