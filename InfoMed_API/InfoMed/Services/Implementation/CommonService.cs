using InfoMed.Data;
using InfoMed.DTO;
using InfoMed.Models;
using InfoMed.Services.Interface;
using log4net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace InfoMed.Services.Implementation
{
    public class CommonService : ICommonService
    {
        private readonly InfoMedContext _dbContext;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public CommonService(InfoMedContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Country>> GetCountries()
        {
            try
            {
                var countries = await _dbContext.Country.ToListAsync();
                return countries;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }
    }
}
