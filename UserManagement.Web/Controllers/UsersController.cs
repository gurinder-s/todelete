using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;


namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]

    public ViewResult List(bool? isActive)
    {
        IEnumerable<User> users;

        if (isActive.HasValue)
        {
            users = _userService.FilterByActive(isActive.Value);
        }
        else
        {
            users = _userService.GetAll();
        }

        var items = users.Select(u => new UserListItemViewModel
        {
            Id = u.Id,
            Forename = u.Forename,
            Surname = u.Surname,
            Email = u.Email,
            IsActive = u.IsActive,
            DateOfBirth = u.DateOfBirth,
        }).ToList();

        var model = new UserListViewModel
        {
            Items = items
        };

        return View(model);
    }

    public object List() => throw new NotImplementedException();

    public async Task<IActionResult> UserDetailViewAsync(int? id)
    {
        var userDetailDto = await _userService.GetUserById(id);
        if (userDetailDto == null)
        {
            return NotFound();
        }
        var userDetailViewModel = new UserDetailViewModel
        {
            Id = userDetailDto.Id,
            Forename = userDetailDto.Forename,
            Surname = userDetailDto.Surname,
            Email = userDetailDto.Email,
            DateOfBirth = userDetailDto.DateOfBirth,
            IsActive = userDetailDto.IsActive
            
        };

        return View(userDetailViewModel);


    }

}
