using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.DataTransferObjects;
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
    [HttpGet("UserDetailView/{id}")]
    public async Task<IActionResult> UserDetailView(int? id)
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

    // GET: Users/Edit/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userDetailDto = await _userService.GetUserById(id);
        if (userDetailDto == null)
        {
            return NotFound();
        }

        var editUserViewModel = new EditUserViewModel
        {
            Id = userDetailDto.Id,
            Forename = userDetailDto.Forename,
            Surname = userDetailDto.Surname,
            Email = userDetailDto.Email,
            DateOfBirth = userDetailDto.DateOfBirth
            // Map other fields as necessary
        };

        return View(editUserViewModel);
    }

    [HttpPost("{id}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditUserViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var userDetailDto = new UserDetailDTO
            {
                Id = model.Id,
                Forename = model.Forename,
                Surname = model.Surname,
                Email = model.Email,
                DateOfBirth = model.DateOfBirth

            };

            var result = await _userService.UpdateUserAsync(userDetailDto);
            if (!result)
            {
                return NotFound(); 
            }

            return RedirectToAction(nameof(UserDetailView), new { id = model.Id });
        }
        return View(model);
    }
}
