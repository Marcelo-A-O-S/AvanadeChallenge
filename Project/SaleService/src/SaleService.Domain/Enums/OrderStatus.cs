namespace SaleService.Domain.Enums
{
    public enum OrderStatus
    {
        PROCESSING,
        CONFIRMED,
        REJECT,
        NOT_FOUND_PRODUCT,
        NOT_STOCK,
        PARTIALLY_CONFIRMED
    }
}