using System.Collections.Generic;
using GraphQL.DataLoader;
using GraphQL.Types;
using WhiskyApp.DataAccess;
using WhiskyApp.DataAccess.Models;

namespace Whisky.GraphQL
{
    public class DestilleryType : ObjectGraphType<DestilleryEntity>
    {
        public DestilleryType(IUnitOfWork unitOfWork, IDataLoaderContextAccessor accessor)
        {
            Field<NonNullGraphType<IdGraphType>>().Name("id");
            Field(entity => entity.Name);
            Field(entity => entity.Description);
            Field(entity => entity.Owner);
            Field(entity => entity.SpiritStills);
            Field(entity => entity.WashStills);
            Field<WhiskyRegionEnumType>("region");

            Field<ListGraphType<WhiskyType>, IEnumerable<WhiskyApp.DataAccess.Models.WhiskyEntity>>()
            .Name("whiskys")
            .ResolveAsync(context => unitOfWork.WhiskyRepository.GetAllByDestilleryIdAsync(context.Source.Id));
        }
    }
}