namespace AliyewRestaurant.Domain.Enums;

public static class OrderStatusExtensions
{
    public static string ToAzeriMessage(this OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => "Sizin sifarişiniz qəbul olundu, tezliklə hazırlanacaq!",
            OrderStatus.Confirmed => "Sizin sifarişiniz təsdiqləndi və mətbəxə göndərildi!",
            OrderStatus.Preparing => "Sizin sifarişiniz hazır olunur, aşpazlarımız iş başındadır!",
            OrderStatus.Completed => "Sifarişiniz tamamlandı, nuş olsun!",
            OrderStatus.Cancelled => "Sizin sifarişiniz ləğv edildi. Üzr istəyirik!",
            _ => "Sifarişinizin statusu dəyişdi."
        };
    }
}