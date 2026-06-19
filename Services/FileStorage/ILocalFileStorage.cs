public interface ILocalFileStorage
{
    Task<ServiceResult<IEnumerable<FileUploadResponseDto>>> SaveAsync(IFormFileCollection files);

    object OpenReadAsync();

    object ExistsAsync();

    object DeleteAsync();
}