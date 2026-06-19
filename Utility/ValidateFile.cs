public static class ValidateFile
{
    // max size 10MB
    private static int MaxSize = 10 * 1024 * 1024;
    private static string[] AllowedExtensionEnum = [
        ".png",
        ".docx",
        ".jpeg",
        ".jpg",
        ".pdf",
    ];
    public static (bool isValid, string ErrorMessage) FileValidator(IFormFile file)
    {

        // check if empty file
        if (file == null || file.Length == 0)
        {
            return (false,"Provide Valid File");
        }

        // check file above configured limit
        if (file.Length >= MaxSize)
        {
            return (false,"Max File Size Reached");
        }

        // check allowed extensions
        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrEmpty(extension) || !AllowedExtensionEnum.Contains(extension,StringComparer.OrdinalIgnoreCase))
        {
            return (false,"File type is not allowed");
        }

        return (true, "Valid file");
    }
}