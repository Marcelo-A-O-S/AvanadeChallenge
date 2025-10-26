namespace SaleService.Domain.Interfaces
{
    public interface IOrderConsumer
    {
        Task StartAsync(CancellationToken cancellationToken);
    }
}