using System.Drawing;

namespace AutoService.DTO
{
    public class TripDTO
    {
        public int RouteId { get; set; }
        public int BusId { get; set; }

        public int DriverId { get; set; }

        public int ConductorId { get; set; }

        public decimal Price { get; set; }

        public DateTime DepTime { get; set; }

        public DateTime ArrTime { get; set; }

    }
}
