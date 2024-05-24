using InfoMed.DTO;
using InfoMed.Models;
using InfoMed.Services.Implementation;
using InfoMed.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InfoMed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TextContentAreaController : ControllerBase
    {
        private readonly ITextContentAreasService _textContentAreasService;

        public TextContentAreaController(ITextContentAreasService textContentAreasService)
        {
            _textContentAreasService = textContentAreasService;
        }

        [HttpGet("GetTextContents")]
        public async Task<ActionResult<List<TextContentAreasDto>>> GetTextContents()
        {
            var textContents = await _textContentAreasService.GetTextContents();
            return Ok(textContents);
        }

        [HttpPost("AddTextContent")]
        public async Task<ActionResult<TextContentAreasDto>> AddTextContent(TextContentAreasDto textContent)
        {
            var result = await _textContentAreasService.AddTextContent(textContent);
            if (result != null) return Ok(result);
            return BadRequest("Error occured while creating!");
        }

        [HttpPost("UpdateTextContent")]
        public async Task<ActionResult<bool>> UpdateTextContent(TextContentAreasDto textContent)
        {
            var result = await _textContentAreasService.UpdateTextContent(textContent);
            if (result != null) return Ok(result);
            return BadRequest("Error occured while updating!");
        }

        [HttpGet("GetTextContentById")]
        public async Task<ActionResult<TextContentAreasDto>> GetTextContentById(int id)
        {
            var textContext = await _textContentAreasService.GetTextContentById(id);
            if (textContext != null) return Ok(textContext);
            return BadRequest("Error occured while fetching data!");
        }

        [HttpGet("GetTextContentByEventVersionId")]
        public async Task<ActionResult<List<TextContentAreasDto>>> GetTextContentByEventVersionId(int versionId)
        {
            var textContext = await _textContentAreasService.GetTextContentByEventVersionId(versionId);
            if (textContext != null) return Ok(textContext);
            return BadRequest("Error occured while fetching data!");
        }
    }
}
