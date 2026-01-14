namespace Contract.Services.V1.Identitys;
public static class Response
{
    public record Authenticated
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }

    public record LoginTokenResponse
    {
        public string AccessToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public CurrentUser CurrentUser { get; set; }
    }

    public record RefreshTokenResponse
    {
        public string AccessToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public CurrentUser CurrentUser { get; set; }
    }

    public record CurrentUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public string UserName { get; set; }
    }
}
