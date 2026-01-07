using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TPMS.API.Common;
using TPMS.Application.Features.Settings.Commands;
using TPMS.Application.Features.Settings.DTOs;
using TPMS.Application.Features.Settings.Queries;

namespace TPMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompanySettingsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    private readonly IWebHostEnvironment _env;
   
   private readonly AppSettings _settings;

    public CompanySettingsController(IMediator mediator,IOptions<AppSettings> settings,
        IWebHostEnvironment env )
    {
        _mediator = mediator;
        _settings = settings.Value;
        _env = env;
        
    } 

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllCompanySettingsQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetCompanySettingsByIdQuery(id));
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CompanySettingsDto dto)
    {
        var id = await _mediator.Send(new CreateCompanySettingsCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id }, new { CompanyID = id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CompanySettingsDto dto)
    {
        dto.CompanyID = id;
        var result = await _mediator.Send(new UpdateCompanySettingsCommand(dto));
        if (!result) return NotFound();
        return Ok(new { Message = "Company settings updated successfully" });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteCompanySettingsCommand(id));
        return result ? Ok(new { Message = "Company settings deleted successfully" }) : NotFound();
    }
    
    //Upload company logo
    /// <summary>
    /// Upload company logo
    /// </summary>
    [HttpPost("logo")]
    public async Task<IActionResult> UploadCompanyLogo(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is required.");

        var allowedTypes = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = Path.GetExtension(file.FileName).ToLower();

        if (!allowedTypes.Contains(extension))
            return BadRequest("Invalid file type.");

        var uploadsFolder = Path.Combine(
            _env.WebRootPath,
            "uploads",
            "company-logos"
        );

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var fileName = $"company_logo_{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        //-- Use configured public URL
        var logoUrl = $"{_settings.PublicBaseUrl}/uploads/company-logos/{fileName}";

        return Ok(new
        {
            logoUrl
        });
    }
    
}