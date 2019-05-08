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
    public class WhiskyInputType : InputObjectGraphType<WhiskyEntity>
    {
        public WhiskyInputType()
        {
            Field(entity => entity.Age).Description("");
            Field(entity => entity.Name).Description("");
            Field(entity => entity.Size).Description("");
            Field(entity => entity.Strength).Description("");
        }
    }
}