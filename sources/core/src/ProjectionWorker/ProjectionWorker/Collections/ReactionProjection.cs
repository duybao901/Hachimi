using ProjectionWorker.Abstractions;

namespace ProjectionWorker.Collections;
public class ReactionProjection : Document
{
    public string Name { get; set; }
    public string Icon { get; set; }
    public string Url { get; set; }
    public int Count { get; set; }
    public bool IsReactionByCurrentUser { get; set; }
}
