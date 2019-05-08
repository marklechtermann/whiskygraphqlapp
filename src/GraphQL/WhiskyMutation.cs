using System.Linq;
using GraphQL;
using GraphQL.Types;
using WhiskyApp.DataAccess;
using WhiskyApp.DataAccess.Models;

namespace Whisky.GraphQL
{
    public class WhiskyMutation : ObjectGraphType
    {
        public WhiskyMutation(IUnitOfWork unitOfWork)
        {
            this.Field<WhiskyType>()
            .Name("addWhisky")
            .Argument<NonNullGraphType<WhiskyInputType>>("whisky", "")
            .Argument<NonNullGraphType<StringGraphType>>("destilleryId", "")
            .ResolveAsync(async context =>
            {

                var entity = context.GetArgument<WhiskyEntity>("whisky");
                var destilleryId = context.GetArgument<string>("destilleryId");

                var destillery = await unitOfWork.DestilleryRepository.GetByIdAsync(destilleryId);
                if (destillery == null)
                {
                    context.Errors.Add(new ExecutionError($"Destillery {destilleryId} not found."));
                    return null;
                }

                entity.DestilleryEntity = destillery;
                var newEntity = await unitOfWork.WhiskyRepository.AddAsync(entity);

                unitOfWork.Save();
                return newEntity;
            }
            );

            this.Field<BooleanGraphType>()
            .Name("deleteWhisky")
            .Argument<IdGraphType>("id", "")
            .ResolveAsync(async context =>
            {
                var id = context.GetArgument<string>("id");
                var whisky = await unitOfWork.WhiskyRepository.GetByIdAsync(id);

                if (whisky == null)
                {
                    context.Errors.Add(new ExecutionError($"Whisky {id} not found."));
                    return false;
                }
                await unitOfWork.WhiskyRepository.DeleteAsync(id);
                unitOfWork.Save();
                return true;
            });
        }
    }
}