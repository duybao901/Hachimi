namespace Contract.Services.V1.Posts.ViewModels;
public class PostListViewModel
{
    public Guid id { get; set; }
    public string title { get; set; }
    public string slug { get; set; }

    public PostAuthorViewModel author { get; set; }
}
