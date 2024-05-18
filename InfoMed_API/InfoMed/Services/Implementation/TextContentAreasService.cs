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
    public class TextContentAreasService : ITextContentAreasService
    {
        private readonly InfoMedContext _dbContext;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly IMapper _mapper;

        public TextContentAreasService(InfoMedContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> AddTextContent(TextContentAreasDto _textContent)
        {
            try
            {
                var textContent = _mapper.Map<TextContentAreas>(_textContent);
                textContent.Status = true;
                await _dbContext.TextContentAreas.AddAsync(textContent);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return false;
            }
        }

        public async Task<TextContentAreasDto> GetTextContentById(int id)
        {
            try
            {
                var textContent = await _dbContext.TextContentAreas.FirstOrDefaultAsync(x => x.IdTextContentArea == id);
                return _mapper.Map<TextContentAreasDto>(textContent);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<List<TextContentAreasDto>> GetTextContents()
        {
            try
            {
                var textContents = await _dbContext.TextContentAreas.ToListAsync();
                return _mapper.Map<List<TextContentAreasDto>>(textContents);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<bool> UpdateTextContent(TextContentAreasDto _textContent)
        {
            try
            {
                var textContent = await _dbContext.TextContentAreas.FirstOrDefaultAsync(x => x.IdTextContentArea == _textContent.IdTextContentArea);
                if (textContent != null)
                {
                    textContent.ContentHeader = _textContent.ContentHeader;
                    textContent.ContentText = _textContent.ContentText;
                    textContent.OrderNumber = _textContent.OrderNumber;
                    textContent.Status = _textContent.Status;
                    _dbContext.TextContentAreas.Update(textContent);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return false;
            }
        }
    }
}
