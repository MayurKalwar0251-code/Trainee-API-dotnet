using System.ComponentModel.DataAnnotations;

public class GetFileResponseDto
{
    public required byte[] FileByte { get; set; }
    public required string ContentType { get; set; }
    public required string fileDownloadName { get; set; }
}