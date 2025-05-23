namespace TestMinimalAPI.Config;

public record AppSettings
{
    public string Environment { get; init; }
    public OAuth OAuth { get; init; }
}

public record OAuth
{
    public string Audience { get; init; }
    public string Domain { get; init;  }
    public string Issuer { get; init; }
}