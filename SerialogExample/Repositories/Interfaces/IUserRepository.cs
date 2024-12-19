using SerialogExample.Models;

namespace SerialogExample.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> Create(User user);
    Task<IEnumerable<User>> GetAll();
}