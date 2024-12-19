using SerialogExample.Models;
using SerialogExample.Models.Exceptions;
using SerialogExample.Repositories.Interfaces;
using SerialogExample.Services.Interfaces;

namespace SerialogExample.Services;

public class UserService: IUserService
{
    private readonly IUserRepository _repository;
    
    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<User> CreateUser(User user)
    {
        ValidateUser(user);
        return await _repository.Create(user);
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _repository.GetAll();
    }

    private void ValidateUser(User user)
    {
        if (string.IsNullOrEmpty(user.Name))
        {
            throw new ValidationException("Nome é obrigatório");
        }

        if (user.Age <= 0)
        {
            throw new ValidationException("Idade deve ser maior que zero");
        }
    }
}