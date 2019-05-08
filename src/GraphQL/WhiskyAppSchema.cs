using GraphQL;
using GraphQL.Types;

namespace Whisky.GraphQL
{
    public class WhiskyAppSchema : Schema
    {
        public WhiskyAppSchema(IDependencyResolver resolver)
        : base(resolver)
        {
            Query = resolver.Resolve<WhiskyQuery>();
            Mutation = resolver.Resolve<WhiskyMutation>();
        }
    }
}