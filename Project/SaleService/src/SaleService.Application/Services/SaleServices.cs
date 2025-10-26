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
        public async Task Delete(Sale entity)
        {
            await this.saleRepository.Delete(entity);
        }

        public async Task<Sale> FindBy(Expression<Func<Sale, bool>> predicate)
        {
            return await this.saleRepository.FindBy(predicate);
        }

        public async Task<Sale> GetById(int Id)
        {
            return await this.saleRepository.GetById(Id);
        }

        public async Task<List<Sale>> List()
        {
            return await this.saleRepository.List();
        }

        public async Task<List<Sale>> List(int? page)
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