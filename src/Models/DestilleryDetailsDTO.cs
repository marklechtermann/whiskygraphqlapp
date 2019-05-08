using WhiskyApp.DataAccess.Models;

namespace WhiskyApp.Models
{
    public class DestilleryDetailsDTO : DestilleryDTO
    {
        public string Owner { get; set; }

        public int SpiritStills { get; set; }

        public int WashStills { get; set; }

        public double Capacity { get; set; }

        public WhiskyRegion Region { get; set; }

        public string Description { get; set; }
    }
}