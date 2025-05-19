using AutoMapper;
using UserService.Models.DAO;
using UserService.Models.DTO;
using UserService.Repositories.Implementations;
using UserService.Repositories.Interfaces;
using UserService.Services.Interfaces;

namespace UserService.Services.Implementations
{
    public class PaymentDataService: IPaymentDataService
    {
        private readonly IPaymentDataRepository _paymentDataRepository;
        private readonly IMapper _mapper;
        public PaymentDataService(IPaymentDataRepository paymentDataRepository,
            IMapper mapper) 
        {
            _paymentDataRepository = paymentDataRepository;
            _mapper = mapper;
        }

        public async Task<List<PaymentDataDto>> GetByUser(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(id));

            var PaymentDataes = await _paymentDataRepository.GetByUser(id);
            return _mapper.Map<List<PaymentDataDto>>(PaymentDataes);
        }

        public async Task<PaymentDataDto> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("PaymentData ID cannot be empty.");
            var paymentData = await _paymentDataRepository.GetById(id);
            return _mapper.Map<PaymentDataDto>(paymentData);
        }

        public async Task<PaymentDataDto> Create(PaymentDataDto paymentDataDto)
        {
            if (paymentDataDto == null)
                throw new ArgumentNullException("PaymentData cannot be null.");

            var paymentData = _mapper.Map<PaymentData>(paymentDataDto);
            var createdPaymentData = await _paymentDataRepository.Create(paymentData);
            return _mapper.Map<PaymentDataDto>(createdPaymentData);
        }

        public async Task Delete(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("PaymentData ID cannot be empty.");
            var PaymentData = await _paymentDataRepository.GetById(id);
            if (PaymentData == null)
                throw new ArgumentException("PaymentData not found.");
            await _paymentDataRepository.Delete(id);
        }
    }
}
