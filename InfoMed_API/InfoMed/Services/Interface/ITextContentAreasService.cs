using InfoMed.DTO;
using InfoMed.Models;

namespace InfoMed.Services.Interface
{
    public interface ITextContentAreasService
    {
        public Task<List<TextContentAreasDto>> GetTextContents();
        public Task<TextContentAreasDto> AddTextContent(TextContentAreasDto textContent);
        public Task<TextContentAreasDto> UpdateTextContent(TextContentAreasDto textContent);
        public Task<TextContentAreasDto> GetTextContentById(int id);
        public Task<List<TextContentAreasDto>> GetTextContentByEventVersionId(int id, int idVersion);
    }
}
