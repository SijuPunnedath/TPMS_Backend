namespace TPMS.Application.Features.Penaltyploicy.DTOs;

public class CreatepenaltyDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal? FixedAmount { get; set; }
    public decimal? PercentageOfRent { get; set; }
    public int GracePeriodDays { get; set; }
    public bool IsActive { get; set; }
}