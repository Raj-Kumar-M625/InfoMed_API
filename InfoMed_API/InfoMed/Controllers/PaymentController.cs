using InfoMed.DTO;
using InfoMed.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfoMed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _payment;

        public PaymentController(IPaymentService payment)
        {
            _payment = payment;
        }

        [HttpGet("GetPaymentDetails")]
        public async Task<ActionResult<PaymentDetailsDto>> GetPaymentDetails(int id, int idVersion)
        {
            var events = await _payment.GetPaymentDetails(id, idVersion);
            return Ok(events);
        }

        [HttpPost("AddPaymentDetails")]
        public async Task<ActionResult<PaymentDetailsDto>> AddPaymentDetails(PaymentDetailsDto paymentDetailsDto)
        {
            var payment = await _payment.AddPaymentDetails(paymentDetailsDto);
            if (payment != null) return Ok(payment);
            return BadRequest("Error occured while fetching data!");
        }
        [HttpPost("UpdatePaymentDetails")]
        public async Task<ActionResult<LastYearMemoryDto>> UpdatePaymentDetails(PaymentDetailsDto paymentDetailsDto)
        {
            var payment = await _payment.UpdatePaymentDetails(paymentDetailsDto);
            if (payment != null) return Ok(payment);
            return BadRequest("Error occured while updating data!");
        }

    
    }
}
