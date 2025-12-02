namespace StockService.Domain.Exceptions
{
    public class InvalidQuantityException : Exception
    {
        public InvalidQuantityException() : base("Quantidade informada inválida")
        {
            
        }
    }
}