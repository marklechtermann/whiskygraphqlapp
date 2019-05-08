using System.Collections.Generic;
using Newtonsoft.Json;

namespace WhiskyApp.Models
{
    public class LinkedResourceBaseDTO
    {
        [JsonProperty(PropertyName = "_link")]
        public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    }
}