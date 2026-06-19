public interface ILocalFileStorage
{
    Task<ServiceResult<IEnumerable<FileUploadResponseDto>>> SaveAsync(IFormFileCollection files);

    Task<ServiceResult<byte[]>> OpenReadAsync(string filePath);

    ServiceResult<bool> ExistsAsync(string path);

    ServiceResult<bool> DeleteAsync(string path);
}