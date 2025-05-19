using UserService.Models.DTO;

namespace UserService.Services.Interfaces
{
    public interface IPaymentDataService
    {
        Task<List<PaymentDataDto>> GetByUser(Guid id);
        Task<PaymentDataDto> GetById(Guid id);
        Task<PaymentDataDto> Create(PaymentDataDto addressDto);
        Task Delete(Guid id);
    }
}
