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

        public async Task<TextContentAreasDto> AddTextContent(TextContentAreasDto _textContent)
        {
            try
            {
                var textContent = _mapper.Map<TextContentAreas>(_textContent);
                //var _event = await _dbContext.EventVersions.FirstOrDefaultAsync(x => x.IdEventVersion == textContent.IdEventVersion);
                //if (_event != null) textContent.IdEvent = _event.IdEvent;
                textContent.Status = true;
                var entity = await _dbContext.TextContentAreas.AddAsync(textContent);
                await _dbContext.SaveChangesAsync();
                return _mapper.Map<TextContentAreasDto>(entity.Entity);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null;
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
        public async Task<List<TextContentAreasDto>> GetTextContentByEventVersionId(int id, int idVersion)
        {
            try
            {
                var textContent = await _dbContext.TextContentAreas
                                                  .Where(x => x.IdEvent == id && x.IdEventVersion == idVersion && x.Status == true)
                                                  .ToListAsync();
                return _mapper.Map<List<TextContentAreasDto>>(textContent);
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
                var textContents = await _dbContext.TextContentAreas.Where(x => x.Status == true).ToListAsync();
                return _mapper.Map<List<TextContentAreasDto>>(textContents);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }
        public async Task<TextContentAreasDto> UpdateTextContent(TextContentAreasDto _textContent)
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
                    var entity = _dbContext.TextContentAreas.Update(textContent);
                    await _dbContext.SaveChangesAsync();
                    return _mapper.Map<TextContentAreasDto>(entity.Entity);
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
