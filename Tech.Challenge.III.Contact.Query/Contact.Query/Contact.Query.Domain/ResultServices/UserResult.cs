﻿using Newtonsoft.Json;

namespace Contact.Query.Domain.ResultServices;
public class UserResult
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("registrationDate")]
    public DateTime RegistrationDate { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    [JsonProperty("password")]
    public string Password { get; set; } = string.Empty;
}
