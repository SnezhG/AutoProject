namespace AutoService.DTO;

public class TripInfoDTO
{
    public int TripId { get; set; }
    public string DepCity { get; set; }
    public string ArrCity { get; set; }
    public string DepTime { get; set; }
    public DateOnly DepDate { get; set; }
    public string ArrTime { get; set; }
    public DateOnly ArrDate { get; set; }
    public decimal Price { get; set; }
}