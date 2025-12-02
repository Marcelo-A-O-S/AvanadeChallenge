using SaleService.Application.Interfaces;
using SaleService.Domain.Entities;
using SaleService.Domain.Interfaces;
using System.Linq.Expressions;

namespace SaleService.Application.Services
{
    public class SaleServices : ISaleServices
    {
        private readonly ISaleRepository saleRepository;
        public SaleServices(ISaleRepository _saleRepository)
        {
            this.saleRepository = _saleRepository;
        }

        public async Task AttachToOrder(long saleId, long orderId)
        {
            await this.saleRepository.AttachToOrder(saleId,orderId);
        }

        public async Task Delete(Sale entity)
        {
            await this.saleRepository.Delete(entity);
        }

        public async Task<List<Sale>> FindAllBy(Expression<Func<Sale, bool>> predicate)
        {
            return await this.saleRepository.FindAllBy(predicate);
        }

        public async Task<Sale> FindBy(Expression<Func<Sale, bool>> predicate)
        {
            return await this.saleRepository.FindBy(predicate);
        }

        public async Task<Sale> GetById(int Id)
        {
            return await this.saleRepository.GetById(Id);
        }

        public async Task<List<Sale>> GetByOrderId(long orderId)
        {
            return await this.saleRepository.GetByOrderId(orderId);
        }

        public async Task<List<Sale>> GetByUserId(long userId,int page = 1, int itemsPage = 10)
        {
            return await this.saleRepository.GetByUserId(userId,page,itemsPage);
        }

        public async Task<int> GetQuantity()
        {
            return await this.saleRepository.GetQuantity();
        }

        public async Task<int> GetQuantityInProgressByUserId(long userId)
        {
            return await this.saleRepository.GetQuantityInProgressByUserId(userId);
        }

        public async Task<List<SalesInfoProduct>> GetSalesInfoProducts( int productInitial, int productFinally)
        {
            return await this.saleRepository.GetSalesInfoProducts(productInitial, productFinally);
        }

        public async Task<List<SalesInfoProduct>> GetSalesInfoProductsByIds(List<long> productIds)
        {
            return await this.saleRepository.GetSalesInfoProductsByIds(productIds);
        }

        public async Task<List<SalesInfoProduct>> GetSalesInfoProductsConfirmedByIds(List<long> productIds)
        {
            return await this.saleRepository.GetSalesInfoProductsConfirmedByIds(productIds);
        }

        public async Task<List<Sale>> GetSalesInProgress(long userId, int page = 1, int itemsPage = 10)
        {
            return await this.saleRepository.GetSalesInProgress(userId,page,itemsPage);
        }

        public async Task<List<Sale>> List()
        {
            return await this.saleRepository.List();
        }

        public async Task<List<Sale>> List(int page)
        {
            return await this.saleRepository.List(page);
        }

        public async Task Save(Sale entity)
        {
            if (entity.Id == 0)
            {
                await this.saleRepository.Save(entity);
            }
            else
            {
                await this.saleRepository.Update(entity);
            }
        }

        public async Task Update(Sale entity)
        {
            await this.saleRepository.Update(entity);
        }
    }
}