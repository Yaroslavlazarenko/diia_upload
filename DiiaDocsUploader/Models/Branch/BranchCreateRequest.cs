using DiiaDocsUploader.Models.Common;

namespace DiiaDocsUploader.Models.Branch;

public class BranchCreateRequest
{
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
    public string OfferRequestType { get; } = "dynamic";

    public Scopes Scopes { get; } = new Scopes
    {
        Sharing =
        [
            "internal-passport",
            "foreign-passport",
            "taxpayer-card",
            "driver-license",
            "vehicle-license",
            "user-birth-record",
            "birth-certificate",
            "reference-internally-displaced-person",
            "student-id-card",
            "pension-card",
            "name-change-act-record",
            "marriage-act-record",
            "divorce-act-record",
            "veteran-certificate",
            "education-document",
            "residence-permit-permanent",
            "residence-permit-temporary"
        ]
    };
}