using Microsoft.EntityFrameworkCore.Storage;

namespace AutoService.DTO;

public class TicketInfoDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public string DepCity { get; set; }
    public string ArrCity { get; set; }
    public DateOnly DepDate { get; set; }
    public string DepTime { get; set; }
    public DateOnly ArrDate { get; set; }
    public string ArrTime { get; set; }
    public sbyte Seat { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; }
}