using System.Drawing;

namespace AutoService.ViewModels
{
    public class TripViewModel
    {
        public int RouteId { get; set; }
        public int BusId { get; set; }

        public int DriverId { get; set; }

        public int CondId { get; set; }

        public decimal Price { get; set; }

        public DateTime DepTime { get; set; }

        public DateTime ArrTime { get; set; }

    }
}
