using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.DataLoader;
using GraphQL.Types;
using WhiskyApp.DataAccess;
using WhiskyApp.DataAccess.Models;

namespace Whisky.GraphQL
{
    public class WhiskyType : ObjectGraphType<WhiskyEntity>
    {
        public WhiskyType(IUnitOfWork unitOfWork, IDataLoaderContextAccessor accessor)
        {
            Field<NonNullGraphType<IdGraphType>>().Name("id");
            Field(entity => entity.Age).Description("");
            Field(entity => entity.Name).Description("");
            Field(entity => entity.Size).Description("");
            Field(entity => entity.Strength).Description("");

            Field<DestilleryType, DestilleryEntity>()
            .Name("destillery")
            .ResolveAsync(context =>
            {
                var loader = accessor.Context.GetOrAddBatchLoader<string, DestilleryEntity>("whisky_destillery", unitOfWork.DestilleryRepository.GetByWhiskyIdsAsync);
                return loader.LoadAsync(context.Source.Id);
            });
        }
    }
}