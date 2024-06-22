namespace Infrastructure.Services.TokenService;

public sealed class TokenSetting
{
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string Secret { get; set; }
    public int Expiration { get; set; }
    public string CookieName { get; set; }
}