using InfoMed.DTO;
using InfoMed.Models;

namespace InfoMed.Services.Interface
{
    public interface ITextContentAreasService
    {
        public Task<List<TextContentAreasDto>> GetTextContents();
        public Task<bool> AddTextContent(TextContentAreasDto textContent);
        public Task<bool> UpdateTextContent(TextContentAreasDto textContent);
        public Task<TextContentAreasDto> GetTextContentById(int id);
    }
}
