using Microsoft.AspNetCore.Authorization;

namespace AliyewRestaurant.Application.Shared.Settings;

public static class Permissions
{
    public static class Category
    {
        public const string Create = "Category.Create";
        public const string Update = "Category.Update";
        public const string Delete = "Category.Delete";
        public const string View = "Category.View";

        public static List<string> All { get; } = new List<string>
        {
            Create,
            Update,
            Delete,
            View
        };
    }

    public static class MenuItem
    {
        public const string Create = "MenuItem.Create";
        public const string Update = "MenuItem.Update";
        public const string Delete = "MenuItem.Delete";
        public const string View = "MenuItem.View";

        public static List<string> All { get; } = new List<string>
        {
            Create,
            Update,
            Delete,
            View
        };
    }

    public static class Order
    {
        public const string Create = "Order.Create";
        public const string Update = "Order.Update";
        public const string Delete = "Order.Delete";
        public const string View = "Order.View";

        public static List<string> All { get; } = new List<string>
        {
            Create,
            Update,
            Delete,
            View
        };
    }

    public static class Reservation
    {
        public const string Create = "Reservation.Create";
        public const string Update = "Reservation.Update";
        public const string Delete = "Reservation.Delete";
        public const string View = "Reservation.View";

 
        public static List<string> All { get; } = new List<string>
        {
            Create,
            Update,
            Delete,
            View
        };
    }

    public static class ReviewPermissions
    {
        public const string Create = "Review.Create";
        public const string Update = "Review.Update";
        public const string Delete = "Review.Delete";
        public const string View = "Review.View";

        public static List<string> All { get; } = new List<string>
    {
        Create,
        Update,
        Delete,
        View
    };
    }

    public static class Role
    {
        public const string Create = "Role.Create";
        public const string Update = "Role.Update";
        public const string Delete = "Role.Delete";
        public const string GetAllPermissions = "Role.GetAllPermissions";

        public static List<string> All { get; } = new List<string>
        {
            GetAllPermissions,
            Create,
            Update,
            Delete
        };
    }
}