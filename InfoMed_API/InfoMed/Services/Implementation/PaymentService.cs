using AutoMapper;
using InfoMed.Data;
using InfoMed.DTO;
using InfoMed.Models;
using InfoMed.Services.Interface;
using log4net;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace InfoMed.Services.Implementation
{
  
    public class PaymentService : IPaymentService
    {
        private readonly InfoMedContext _dbContext;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly IMapper _mapper;

        public PaymentService(InfoMedContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<PaymentDetailsDto> GetPaymentDetails(int id, int idVersion)
        {
            try
            {
                var events = await _dbContext.PaymentDetails.Where(x => x.IdEvent == id && x.IdEventVersion == idVersion).FirstOrDefaultAsync();
                return _mapper.Map<PaymentDetailsDto>(events);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<PaymentDetailsDto> AddPaymentDetails(PaymentDetailsDto paymentDetailsDto)
        {
            try
            {
                PaymentDetails paymentDetails = _mapper.Map<PaymentDetails>(paymentDetailsDto);                
                var paymentEntity = await _dbContext.PaymentDetails.AddAsync(paymentDetails);
                await _dbContext.SaveChangesAsync();
                return _mapper.Map<PaymentDetailsDto>(paymentEntity.Entity);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }
        

        public async Task<PaymentDetailsDto> UpdatePaymentDetails(PaymentDetailsDto paymentDetailsDto)
        {
            try
            {
                var payment = await _dbContext.PaymentDetails
                                                     .FirstOrDefaultAsync(x => x.IdPaymentArea == paymentDetailsDto.IdPaymentArea);
                if (payment != null)
                {
                    payment.PaymentTextContent = paymentDetailsDto.PaymentTextContent;
                    payment.QRCodeImage = paymentDetailsDto.QRCodeImage;                    
                    var paymentEntity = _dbContext.PaymentDetails.Update(payment);
                    await _dbContext.SaveChangesAsync();
                    return _mapper.Map<PaymentDetailsDto>(paymentEntity.Entity);
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }


    }
}
