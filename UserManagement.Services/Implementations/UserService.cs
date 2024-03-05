
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.DataTransferObjects;


namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public IEnumerable<User> FilterByActive(bool isActive)
    {

        return _dataAccess.GetAll<User>().Where(u => u.IsActive == isActive).ToList();
    }

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();

    public async Task<UserDetailDTO?> GetUserById(int? Id) {
        var user = await _dataAccess.GetAll<User>().FirstOrDefaultAsync(u => u.Id == Id);
        if (user == null) { return null; }
        var viewModel = new UserDetailDTO
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth,

        };
        return viewModel;
    }
}
