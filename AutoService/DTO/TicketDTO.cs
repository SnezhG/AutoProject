using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace AutoService.DTO
{
    public class TicketDTO
    {
        /*public int clientId { get; set; }*/
        public string LastName { get; set; }

        public string Name { get; set; }

        public string? Patronymic { get; set; }

        public string Sex { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string PassSeries { get; set; }

        public string PassNum { get; set; }

        /*[Phone]*/
        public string PhoneNum { get; set; }

        public int Seat { get; set; }
        
        public int Trip { get; set; }
    }
}