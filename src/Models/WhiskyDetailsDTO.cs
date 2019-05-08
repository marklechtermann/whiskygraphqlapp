using Microsoft.EntityFrameworkCore;

namespace WhiskyApp.Models
{
    public class WhiskyDetailsDTO : WhiskyDTO
    {
        public int Age { get; set; }

        public double Strength { get; set; }

        public int Size { get; set; }
    }
}