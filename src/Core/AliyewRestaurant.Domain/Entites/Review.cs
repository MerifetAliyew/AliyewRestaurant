namespace AliyewRestaurant.Domain.Entites;

public class Review : BaseEntity
{
    public Guid MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; }

    public string UserId { get; set; }
    public AppUser User { get; set; }

    public string CommentBody { get; set; }
    public int Rating { get; set; } // 1-5 ulduz
    public bool IsConfirmed { get; set; } = false;
    public DateTime? ConfirmedAt { get; set; }
}
