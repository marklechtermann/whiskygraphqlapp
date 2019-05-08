using System.Linq;
using GraphQL.Types;
using WhiskyApp.DataAccess;

namespace Whisky.GraphQL
{
    public class WhiskyQuery : ObjectGraphType
    {
        public WhiskyQuery(IUnitOfWork unitOfWork)
        {
            //Todo: Async!

            this.Field<NonNullGraphType<ListGraphType<NonNullGraphType<WhiskyType>>>>(
                name: "whiskys",
                resolve: r => unitOfWork.WhiskyRepository.GetAllAsync().GetAwaiter().GetResult());

            this.Field<WhiskyType>(
                name: "whisky",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>>() { Name = "id" }),
                resolve: context => unitOfWork.WhiskyRepository.GetByIdAsync(context.GetArgument<string>("id")).GetAwaiter().GetResult());

            this.Field<NonNullGraphType<ListGraphType<NonNullGraphType<DestilleryType>>>>(
                name: "destillery",
                resolve: r => unitOfWork.DestilleryRepository.GetAllAsync().GetAwaiter().GetResult());
        }
    }
}