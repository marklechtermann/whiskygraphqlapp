using System.Collections.Generic;

namespace WhiskyApp.DataAccess.Models
{
    public class DestilleryEntity : IEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Owner { get; set; }

        public int SpiritStills { get; set; }

        public int WashStills { get; set; }

        public double Capacity { get; set; }

        public string Description { get; set; }

        public WhiskyRegion Region { get; set; }

        public List<WhiskyEntity> Whiskys { get; set; }

    }
}