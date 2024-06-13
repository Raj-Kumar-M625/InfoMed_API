using InfoMed.DTO;

namespace InfoMed.Services.Interface
{
    public interface IPaymentService
    {
        public Task<PaymentDetailsDto> GetPaymentDetails(int id, int idVersion);
        public Task<PaymentDetailsDto> AddPaymentDetails(PaymentDetailsDto paymentDetailsDto);
        public Task<PaymentDetailsDto> UpdatePaymentDetails(PaymentDetailsDto paymentDetailsDto);   
    }
}
