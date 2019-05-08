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
    [Route("api/whiskys")]
    public class WhiskyController : Controller
    {
        private readonly IUrlHelper urlHelper;

        private readonly IUnitOfWork unitOfWork;

        public WhiskyController(IUnitOfWork unitOfWork, IUrlHelper urlHelper)
        {
            this.unitOfWork = unitOfWork;
            this.urlHelper = urlHelper;
        }

        [HttpGet("", Name = nameof(GetWhiskys))]
        public async Task<IEnumerable<WhiskyDTO>> GetWhiskys()
        {
            var whiskys = await this.unitOfWork.WhiskyRepository.GetAllAsync();

            return whiskys.Select(w =>
            {
                return CreateLinks(new WhiskyDTO()
                {
                    Id = w.Id,
                    Name = w.Name,
                });
            }
            );
        }

        [HttpGet("{id}", Name = nameof(GetWhisky))]
        public async Task<WhiskyDetailsDTO> GetWhisky(string id)
        {
            var whisky = await this.unitOfWork.WhiskyRepository.GetByIdAsync(id);
            var distillery = await this.unitOfWork.DestilleryRepository.GetByWhiskyIdAsync(whisky.Id);

            return CreateLinks(new WhiskyDetailsDTO()
            {
                Id = whisky.Id,
                Name = whisky.Name,
                Age = whisky.Age,
                Size = whisky.Size,
                Strength = whisky.Strength
            }, new DestilleryDTO()
            {
                Id = distillery.Id,
                Name = distillery.Name
            });
        }

        private WhiskyDTO CreateLinks(WhiskyDTO whisky)
        {
            var idObj = new { id = whisky.Id };
            whisky.Links.Add(
                new LinkDTO(this.urlHelper.Link(nameof(this.GetWhisky), idObj),
                "whisky_detail",
                "GET"));

            return whisky;
        }

        private WhiskyDetailsDTO CreateLinks(WhiskyDetailsDTO whisky, DestilleryDTO destillery = null)
        {
            var idObj = new { id = whisky.Id };
            whisky.Links.Add(
                new LinkDTO(this.urlHelper.Link(nameof(this.GetWhisky), idObj),
                "self",
                "GET"));


            if (destillery != null)
            {
                var idObjDest = new { id = destillery.Id };
                whisky.Links.Add(
                    new LinkDTO(this.urlHelper.Link(nameof(DestilleryController.GetDestillery), idObjDest),
                    "destillery",
                    "GET"));
            }

            return whisky;
        }
    }
}
