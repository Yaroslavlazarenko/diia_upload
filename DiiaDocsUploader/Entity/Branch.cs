namespace DiiaDocsUploader.Entity;

public class Branch
{
    public string Id { get; init; } = null!;
    public string? CustomFullName { get; init; }
    public string? CustomFullAddress { get; init; }
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Region { get; init; } = null!;
    public string District { get; init; } = null!;
    public string Location { get; init; } = null!;
    public string Street { get; init; } = null!;
    public string House { get; init; } = null!;
    public string[] DeliveryTypes { get; } = ["api"];
    public string OfferRequestType => "dynamic";

    public ICollection<BranchDocumentType> BranchDocumentTypes { get; init;} = new HashSet<BranchDocumentType>();
}