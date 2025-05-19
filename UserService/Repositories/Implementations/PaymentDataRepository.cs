using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models.DAO;
using UserService.Repositories.Interfaces;

namespace UserService.Repositories.Implementations
{
    public class PaymentDataRepository: IPaymentDataRepository
    {
        public readonly UserServiceContext _context;
        public PaymentDataRepository(UserServiceContext context) {
            _context = context;
        }

        public async Task<List<PaymentData>> GetByUser(Guid id)
        {
            return await _context.PaymentDatas.Where(a => a.UserId == id).ToListAsync();
        }

        public async Task<PaymentData> GetById(Guid id)
        {
            return await _context.PaymentDatas.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<PaymentData> Create(PaymentData paymentData)
        {
            await _context.PaymentDatas.AddAsync(paymentData);
            await _context.SaveChangesAsync();
            return paymentData;
        }

        public async Task<PaymentData> Update(PaymentData paymentData)
        {
            _context.PaymentDatas.Update(paymentData);
            await _context.SaveChangesAsync();
            return paymentData;
        }

        public async Task Delete(Guid id)
        {
            var paymentData = await GetById(id);
            if (paymentData != null)
            {
                _context.PaymentDatas.Remove(paymentData);
                await _context.SaveChangesAsync();
            }
        }
    }
}
