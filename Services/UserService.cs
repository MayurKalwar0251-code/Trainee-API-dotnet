using Microsoft.AspNetCore.Mvc;
using TrainineeAPI.Models;

class UserService : IUserService
{
    private readonly TraineeContext _traineeContext;
    public UserService(TraineeContext traineeContext)
    {
        _traineeContext = traineeContext;
    }
    IActionResult IUserService.Login()
    {
        throw new NotImplementedException();
    }

    IActionResult IUserService.SignUp()
    {
        throw new NotImplementedException();
    }
}