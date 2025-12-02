namespace StockService.Domain.Enums
{
    public enum OrderStatusResponse
    {
        PROCESSING,
        CONFIRMED,
        REJECT,
        NOT_FOUND_PRODUCT,
        NOT_STOCK,
        PARTIALLY_CONFIRMED
    }
}