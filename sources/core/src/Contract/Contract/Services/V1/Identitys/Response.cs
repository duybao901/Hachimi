namespace Contract.Services.V1.Identity;
public static class Response
{
    public record Authenticated
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
    }

    public record LoginTokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }

    }

    public record RefreshTokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }

    }
}
