using DataAccess;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Business
{
    public class UserService
    {
        private readonly UserRepository _repository;

        public UserService()
        {
            _repository = new UserRepository();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _repository.GetUsersAsync();
        }
        public async Task<bool> AddUserAsync(User user)
        {
            return await _repository.AddUserAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await _repository.DeleteUserAsync(userId);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            return await _repository.UpdateUserAsync(user);
        }

    }
}
