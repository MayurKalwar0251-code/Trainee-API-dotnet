public static class PasswordUtility
{
    public static string HashUserPassword(string plaintextPassword)
    {
        // Automatically manages salting and outputs a single combined string
        return BCrypt.Net.BCrypt.HashPassword(plaintextPassword);
    }
    public static bool VerifyUserPassword(string inputPassword, string storedHashFromDatabase)
    {
        // Returns true if match, false if invalid
        return BCrypt.Net.BCrypt.Verify(inputPassword, storedHashFromDatabase);
    }
}