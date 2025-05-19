using UserService.Models.DAO;

namespace UserService.Repositories.Interfaces
{
    public interface IPaymentDataRepository
    {
        public Task<List<PaymentData>> GetByUser(Guid id);
        public Task<PaymentData> GetById(Guid id);
        public Task<PaymentData> Create(PaymentData paymentData);
        public Task<PaymentData> Update(PaymentData paymentData);
        public Task Delete(Guid id);
    }
}
