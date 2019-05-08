using Microsoft.EntityFrameworkCore;

namespace WhiskyApp.Models
{
    public class WhiskyDTO : LinkedResourceBaseDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

    }
}