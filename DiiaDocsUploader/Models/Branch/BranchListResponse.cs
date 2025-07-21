using System.Text.Json.Serialization;

namespace DiiaDocsUploader.Models.Branch;

public class BranchListResponse
{
    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("branches")]
    public List<BranchResponse> Branches { get; set; } = null!;
}