using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WhiskyApp.DataAccess;
using WhiskyApp.DataAccess.Models;
using WhiskyApp.Models;

namespace WhiskyApp.Controllers
{
    [Route("api/destilleries")]
    public class DestilleryController : Controller
    {
        private readonly IUrlHelper urlHelper;

        private readonly IUnitOfWork unitOfWork;

        public DestilleryController(IUnitOfWork unitOfWork, IUrlHelper urlHelper)
        {
            this.unitOfWork = unitOfWork;
            this.urlHelper = urlHelper;
        }

        [HttpGet("", Name = nameof(GetDestilleries))]
        public async Task<IEnumerable<DestilleryDTO>> GetDestilleries()
        {
            var destilleries = await this.unitOfWork.DestilleryRepository.GetAllAsync();

            return destilleries.Select(w =>
            {
                return CreateLinks(new DestilleryDTO()
                {
                    Id = w.Id,
                    Name = w.Name,
                });
            }
            );
        }

        [HttpGet("{id}", Name = nameof(GetDestillery))]
        public async Task<DestilleryDetailsDTO> GetDestillery(string id)
        {
            var destillery = await this.unitOfWork.DestilleryRepository.GetByIdAsync(id);

            return CreateLinks(new DestilleryDetailsDTO()
            {
                Id = destillery.Id,
                Name = destillery.Name,
                Capacity = destillery.Capacity,
                Owner = destillery.Owner,
                Region = destillery.Region,
                SpiritStills = destillery.SpiritStills,
                WashStills = destillery.WashStills,
                Description = destillery.Description
            });
        }

        [HttpGet("{id}/whiskys", Name = nameof(GetWhiskysFromDestillery))]
        public async Task<IEnumerable<WhiskyDTO>> GetWhiskysFromDestillery(string id)
        {
            var whiskys = await this.unitOfWork.WhiskyRepository.GetAllByDestilleryIdAsync(id);

            return whiskys.Select(w =>
         {
             return CreateLinks(new WhiskyDTO()
             {
                 Id = w.Id,
                 Name = w.Name,
             }, "whisky_details");
         }
         );
        }

        private WhiskyDTO CreateLinks(WhiskyDTO whisky, string rel = "self")
        {
            var idObj = new { id = whisky.Id };
            whisky.Links.Add(
                new LinkDTO(this.urlHelper.Link(nameof(WhiskyController.GetWhisky), idObj),
                rel,
                "GET"));

            return whisky;
        }

        private DestilleryDTO CreateLinks(DestilleryDTO destillery)
        {
            var idObj = new { id = destillery.Id };
            destillery.Links.Add(
                new LinkDTO(this.urlHelper.Link(nameof(this.GetDestillery), idObj),
                "destillery_detail",
                "GET"));

            return destillery;
        }

        private DestilleryDetailsDTO CreateLinks(DestilleryDetailsDTO destillery)
        {
            var idObj = new { id = destillery.Id };
            destillery.Links.Add(
                new LinkDTO(this.urlHelper.Link(nameof(this.GetDestillery), idObj),
                "self",
                "GET"));

            destillery.Links.Add(
                new LinkDTO(this.urlHelper.Link(nameof(this.GetWhiskysFromDestillery), idObj),
                "destillery_whiskys",
                "GET"));

            return destillery;
        }
    }
}
