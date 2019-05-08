namespace WhiskyApp.DataAccess.Models
{
    public class WhiskyEntity : IEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public double Strength { get; set; }

        public int Size { get; set; }

        public DestilleryEntity DestilleryEntity { get; set; }
    }
}