using Ardalis.SmartEnum;

public sealed class PostStatus : SmartEnum<PostStatus>
{
    public static readonly PostStatus Draft = new PostStatus(nameof(Draft), 1);
    public static readonly PostStatus Review = new PostStatus(nameof(Review), 2);
    public static readonly PostStatus Published = new PostStatus(nameof(Published), 3);

    private PostStatus(string name, int value) : base(name, value) { }

    // Property for EF Core mapping
    public int Value => base.Value;
}
