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
    public object DeleteAsync()
    {
        throw new NotImplementedException();
    }

    public object ExistsAsync()
    {
        throw new NotImplementedException();
    }

    public object OpenReadAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResult<IEnumerable<FileUploadResponseDto>>> SaveAsync(IFormFileCollection files)
    {
        Console.WriteLine("LocalFile Storage function");
        long size = files.Sum(f => f.Length);

        List<FileUploadResponseDto> uploadedFiles = [];

        foreach (var file in files)
        {
            if (file.Length > 0) {
                var randomName =  Path.GetRandomFileName();
                var filePath = Path.Combine(_configuration["StoredFilesPath"]!, randomName + Path.GetExtension(file.FileName));
                
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
                    GeneratedStorageName = randomName,
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