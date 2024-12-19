using SerialogExample.Models;
using SerialogExample.Repositories.Interfaces;

namespace SerialogExample.Repositories;

public class UserRepository: IUserRepository
{
    private static readonly List<User> _users = new();
    
    public async Task<User> Create(User user)
    {
        _users.Add(user);
        return await Task.FromResult(user);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await Task.FromResult(_users);
    }
}