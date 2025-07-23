namespace DiiaDocsUploader.Models.FileSystem;

public class ProcessingResult
{
    public bool IsSuccess { get; init; }
    public Guid DeepLinkId { get; init; }
    public string? ErrorMessage { get; init; }

    public static ProcessingResult Success(Guid deepLinkId) 
        => new() { IsSuccess = true, DeepLinkId = deepLinkId };

    public static ProcessingResult Failure(string errorMessage) 
        => new() { IsSuccess = false, ErrorMessage = errorMessage };
}