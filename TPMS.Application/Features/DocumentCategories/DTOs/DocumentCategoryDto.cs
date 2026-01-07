using System;

namespace TPMS.Application.Features.DocumentCategory.DTOs;

public class DocumentCategoryDto
{
    public int DocumentCategoryID { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    
    public bool IsDeleted { get; set; } = false;
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

}