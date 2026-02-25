namespace Contract.Services.V1.Posts.ViewModels;
public class PostReactionViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    public int Count { get; set; }
    public string Url { get; set; }
}
