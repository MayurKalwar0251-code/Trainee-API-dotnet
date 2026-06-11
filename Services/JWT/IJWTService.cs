using TrainineeAPI.Models;
public interface IJWTService
{
    string GenerateToken(User loginUserDto);
}
