namespace Contract.Services.V1.Posts.ViewModels;

public class PostAuthorViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string AvatarUrl { get; set; }
}