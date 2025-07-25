﻿using System.Text.Json.Serialization;

namespace DiiaDocsUploader.Credentials;

public sealed class DiiaCredentials
{
    public string Host { get; init; } = null!;
    public string AcquirerToken { get; init; } = null!;
    
    public string? DefaultReturnLink { get; init; }
    public string? AuthAcquirerToken { get; init; }
}

public class Root
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("project_id")]
    public string ProjectId { get; set; }

    [JsonPropertyName("private_key_id")]
    public string PrivateKeyId { get; set; }

    [JsonPropertyName("private_key")]
    public string PrivateKey { get; set; }

    [JsonPropertyName("client_email")]
    public string ClientEmail { get; set; }

    [JsonPropertyName("client_id")]
    public string ClientId { get; set; }

    [JsonPropertyName("auth_uri")]
    public string AuthUri { get; set; }

    [JsonPropertyName("token_uri")]
    public string TokenUri { get; set; }

    [JsonPropertyName("auth_provider_x509_cert_url")]
    public string AuthProviderX509CertUrl { get; set; }

    [JsonPropertyName("client_x509_cert_url")]
    public string ClientX509CertUrl { get; set; }

    [JsonPropertyName("universe_domain")]
    public string UniverseDomain { get; set; }
}