namespace AuthorizationAPI.DependencyInjection.Options;
public class MasstransitConfigurationOption
{
    public string Host { get; set; }
    public string VHost { get; set; }
    public ushort Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
