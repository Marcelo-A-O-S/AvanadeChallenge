using Microsoft.EntityFrameworkCore;
using SaleService.Domain.Entities;
using SaleService.Domain.Enums;
using SaleService.Domain.Interfaces;
using SaleService.Infrastructure.Context;
using System.Collections.Generic;
namespace SaleService.Infrastructure.Repositories
{
    public class SaleRepository : Generics<Sale>, ISaleRepository
    {
        private readonly DBContext context;
        public SaleRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task AttachToOrder(long saleId, long orderId)
        {
            var sale = new Sale { Id = saleId };
            this.context.Attach(sale);
            sale.OrderId = orderId;
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Sale>> GetByOrderId(long orderId)
        {
            return await this.context.Sales.Where(s=> s.OrderId == orderId).ToListAsync();
        }

        public async Task<List<Sale>> GetByUserId(long userId, int page = 1, int itemsPage = 10)
        {
            var query = this.context.Sales.AsQueryable();
            var items = await query.Where(s=> s.UserId == userId).Skip((page - 1) * itemsPage).ToListAsync();
            return items;
        }

        public async Task<int> GetQuantity()
        {
            return await this.context.Sales.CountAsync();
        }

        public async Task<int> GetQuantityInProgressByUserId(long userId)
        {
            return await this.context.Sales.Where(s=> s.UserId == userId && s.Status == SaleStatus.PARTIALLY_CONFIRMED).CountAsync();
        }

        public async Task<List<SalesInfoProduct>> GetSalesInfoProducts(int productInitial, int productFinally)
        {
            return await this.context.Sales.Where(s => s.ProductId >= productInitial && s.ProductId <= productFinally).GroupBy(s => s.ProductId)
            .Select(g => new SalesInfoProduct { ProductId = g.Key, TotalSales = g.Sum(x => x.Quantity) })
            .ToListAsync();
        }

        public async Task<List<SalesInfoProduct>> GetSalesInfoProductsByIds(List<long> productIds)
        {
            return await this.context.Sales.Where(s => productIds.Contains(s.ProductId) ).GroupBy(s=> s.ProductId)
            .Select(g => new SalesInfoProduct { ProductId = g.Key, TotalSales = g.Sum(x => x.Quantity) })
            .ToListAsync();
        }

        public async Task<List<SalesInfoProduct>> GetSalesInfoProductsConfirmedByIds(List<long> productIds)
        {
            return await this.context.Sales.Where(s => productIds.Contains(s.ProductId) && s.Status == SaleStatus.CONFIRMED).GroupBy(s=> s.ProductId)
            .Select(g => new SalesInfoProduct { ProductId = g.Key, TotalSales = g.Sum(x => x.Quantity) })
            .ToListAsync();
        }

        public async Task<List<Sale>> GetSalesInProgress(long userId, int page = 1, int itemsPage = 10)
        {
            var query = this.context.Sales.AsQueryable();
            var items = await query.Where(s=> s.UserId == userId && s.Status != SaleStatus.CONFIRMED && s.OrderId == null)
                .Skip((page - 1) * itemsPage).ToListAsync();
            return items;
        }
    }
}