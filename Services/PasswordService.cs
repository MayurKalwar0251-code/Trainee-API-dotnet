using BCrypt.Net;

public class PasswordService
{
    // 2. Method used during login to verify the credentials
    public bool VerifyUserPassword(string inputPassword, string storedHashFromDatabase)
    {
        // Returns true if match, false if invalid
        return BCrypt.Net.BCrypt.Verify(inputPassword, storedHashFromDatabase);
    }
}