using StockService.Application.Interfaces;
using StockService.Domain.Entities;
using StockService.Domain.Interfaces;
using System.Linq.Expressions;
namespace StockService.Application.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository productRepository;
        public ProductServices(IProductRepository _productRepository)
        {
            this.productRepository = _productRepository;
        }
        public async Task Delete(Product entity)
        {
            await this.productRepository.Delete(entity);
        }

        public async Task<Product> FindBy(Expression<Func<Product, bool>> predicate)
        {
            return await this.productRepository.FindBy(predicate);
        }

        public async Task<Product> GetById(long Id)
        {
            return await this.productRepository.GetById(Id);
        }

        public async Task<List<Product>> List()
        {
            return await this.productRepository.List();
        }

        public async Task<List<Product>> List(int? page)
        {
            return await this.productRepository.List(page);
        }

        public async Task Save(Product entity)
        {
            if (entity.Id == 0)
            {
                await this.productRepository.Save(entity);
            }
            else
            {
                await this.productRepository.Update(entity);
            }
        }

        public async Task Update(Product entity)
        {
            await this.productRepository.Update(entity);
        }
    }
}