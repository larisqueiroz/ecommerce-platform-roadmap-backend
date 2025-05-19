using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Models.DTO;
using UserService.Repositories.Interfaces;
using UserService.Services.Interfaces;

namespace UserService.Controllers
{
    [ApiController]
    [Route("/scalar/v1/paymentdatas")]
    [Authorize("Users")]
    public class PaymentDataController: ControllerBase
    {
        private readonly IPaymentDataService _paymentDataService;
        private readonly IMapper _mapper;
        public PaymentDataController(IMapper mapper, IPaymentDataService paymentDataService)
        {
            _mapper = mapper;
            _paymentDataService = paymentDataService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var address = await _paymentDataService.GetById(id);
                return Ok(address);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("by-user")]
        public async Task<IActionResult> GetByUser(Guid id)
        {
            try
            {
                var addresses = await _paymentDataService.GetByUser(id);
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentDataDto paymentDataDto)
        {
            try
            {
                var createdAddress = await _paymentDataService.Create(paymentDataDto);
                return Ok(createdAddress);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _paymentDataService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
