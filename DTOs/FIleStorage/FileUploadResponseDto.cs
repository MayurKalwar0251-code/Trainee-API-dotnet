using System.ComponentModel.DataAnnotations;

public class FileUploadResponseDto()
{
    public required string OriginalFileName { get; set; }
    public required string GeneratedStorageName { get; set; }
    public required string ContentType { get; set; }
    public required string Checksum { get; set; }
    public required long Size { get; set; }
}