using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Models.DTO;
using UserService.Services.Interfaces;

namespace UserService.Controllers
{
    [ApiController]
    [Authorize("Users")]
    [Route("scalar/v1/addresses")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("by-id")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            try
            {
                var address = await _addressService.GetById(id);
                return Ok(address);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("by-userid")]
        public async Task<ActionResult<List<AddressDto>>> GetAllByUser([FromQuery] Guid id)
        {
            try
            {
                List<AddressDto> addresses = await _addressService.GetByUser(id);
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddressDto addressDto)
        {
            try
            {
                var createdAddress = await _addressService.Create(addressDto);
                return Ok(createdAddress);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AddressDto addressDto)
        {
            try
            {
                var updatedAddress = await _addressService.Update(addressDto);
                return Ok(updatedAddress);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            try
            {
                await _addressService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
