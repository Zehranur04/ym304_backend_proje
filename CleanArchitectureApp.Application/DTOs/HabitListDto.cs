using CleanArchitectureApp.Domain.Enums;

namespace CleanArchitectureApp.Application.DTOs;

public class HabitListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public decimal? TargetValue { get; set; }
    public UnitType? Unit { get; set; }
    public TrackingType TrackingType { get; set; }
    public FrequencyType Frequency { get; set; }
    public decimal CurrentValue { get; set; }
    public bool CompletedThisPeriod { get; set; }  // Günlük: bugün tamamlandı mı; Haftalık: bu hafta tamamlandı mı
    public int? DaysUntilAvailable { get; set; }   // Haftalık: tekrar yapılabilmesi için kalan gün sayısı
}
