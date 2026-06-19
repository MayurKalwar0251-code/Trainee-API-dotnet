using System.Security.Cryptography;
using System.Threading.Tasks;
using TrainineeAPI.Models;

public class LocalFileStorage : ILocalFileStorage
{
    private readonly IConfiguration _configuration;
    public LocalFileStorage(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public ServiceResult<bool> DeleteAsync(string path)
    {
        File.Delete(path);
        return ServiceResult<bool>.Ok(true);
    }

    public ServiceResult<bool> ExistsAsync(string path)
    {
        if (!File.Exists(path))
        {
            return ServiceResult<bool>.Fail("File Not Found");
        }else
        {
            return ServiceResult<bool>.Ok(true);
        }
    }

    public async Task<ServiceResult<byte[]>> OpenReadAsync(string filePath)
    {
        byte[] fileBytes = await File.ReadAllBytesAsync(filePath);

        return ServiceResult<byte[]>.Ok(fileBytes);
    }

    public async Task<ServiceResult<IEnumerable<FileUploadResponseDto>>> SaveAsync(IFormFileCollection files)
    {
        Console.WriteLine("LocalFile Storage function");
        long size = files.Sum(f => f.Length);

        List<FileUploadResponseDto> uploadedFiles = [];

        foreach (var file in files)
        {
            if (file.Length > 0) {
                var generatedStorageName =  Path.GetRandomFileName() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(_configuration["StoredFilesPath"]!, generatedStorageName);
                
                Console.WriteLine("PathNameeee" + filePath);

                string checksum = "";

                using( var sha = SHA256.Create())

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                    var hashBytes = await sha.ComputeHashAsync(file.OpenReadStream());
                    checksum = BitConverter.ToString(hashBytes).Replace("-","").ToLowerInvariant();
                    Console.WriteLine("Check sum calc : " + checksum);
                }

                FileUploadResponseDto fileUploadDto = new FileUploadResponseDto
                {
                    OriginalFileName = file.FileName,
                    GeneratedStorageName = generatedStorageName,
                    ContentType = file.ContentType,
                    Checksum = checksum,
                    Size = file.Length,
                };

                uploadedFiles.Add(fileUploadDto);
            }
        }

        Console.WriteLine("LocalFile Storage function No error");

        return ServiceResult<IEnumerable<FileUploadResponseDto>>.Ok(uploadedFiles,"Uplaoded all files successfully");
    }
}