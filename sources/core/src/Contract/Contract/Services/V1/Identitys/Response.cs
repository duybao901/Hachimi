namespace Contract.Services.V1.Identity;
public static class Response
{
    public record Authenticated
    {
        public string AccessToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
