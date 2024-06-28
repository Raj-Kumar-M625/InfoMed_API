using InfoMed.Models;

namespace InfoMed.Services.Interface
{
    public interface ICommonService
    {
        Task<List<Country>> GetCountries();
    }
}
