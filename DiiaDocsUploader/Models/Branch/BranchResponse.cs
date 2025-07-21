using System.Text.Json.Serialization;
using DiiaDocsUploader.Models.Common;

namespace DiiaDocsUploader.Models.Branch;

public class BranchResponse
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    [JsonPropertyName("region")]
    public string Region { get; set; } = null!;

    [JsonPropertyName("district")]
    public string District { get; set; } = null!;

    [JsonPropertyName("location")]
    public string Location { get; set; } = null!;

    [JsonPropertyName("street")]
    public string Street { get; set; } = null!;

    [JsonPropertyName("house")]
    public string House { get; set; } = null!;

    [JsonPropertyName("scopes")]
    public Scopes Scopes { get; set; } = null!;

    [JsonPropertyName("customFullName")]
    public string? CustomFullName { get; set; }

    [JsonPropertyName("customFullAddress")]
    public string? CustomFullAddress { get; set; }

    [JsonPropertyName("deliveryTypes")]
    public List<string> DeliveryTypes { get; set; } = null!;

    [JsonPropertyName("offerRequestType")]
    public string OfferRequestType { get; set; } = null!;
}