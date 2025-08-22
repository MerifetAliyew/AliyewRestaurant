namespace AliyewRestaurant.Domain.Entites;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }

    public Guid? ParentCategoryId { get; set; }
    public Category ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; }

    public ICollection<MenuItem> MenuItems { get; set; }
}
