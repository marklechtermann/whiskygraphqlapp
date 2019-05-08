using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Http;
using GraphQL.Server.Internal;
using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Server.Transports.AspNetCore.Common;
using GraphQL.Types;
using GraphQL.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Whisky.GraphQL
{

    public class GraphQLMiddleware
    {
        private readonly RequestDelegate next;
        private readonly GraphQLSettings settings;
        private readonly IDocumentExecuter executer;
        private readonly IDocumentWriter writer;
        private DataLoaderDocumentListener listener;

        public GraphQLMiddleware(
            RequestDelegate next,
            GraphQLSettings settings,
            IDocumentExecuter executer,
            IDocumentWriter writer,
            DataLoaderDocumentListener listener)
        {
            this.next = next;
            this.settings = settings;
            this.executer = executer;
            this.writer = writer;
            this.listener = listener;
        }

        public async Task Invoke(HttpContext context, ISchema schema)
        {
            if (!IsGraphQLRequest(context))
            {
                await next(context);
                return;
            }

            await ExecuteAsync(context, schema);
        }

        private bool IsGraphQLRequest(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments(settings.Path)
                && string.Equals(context.Request.Method, "POST", StringComparison.OrdinalIgnoreCase);
        }

        private async Task ExecuteAsync(HttpContext context, ISchema schema)
        {
            var request = Deserialize<GraphQLRequest>(context.Request.Body);

            var result = await executer.ExecuteAsync(config =>
            {
                config.Listeners.Add(listener);
                config.Schema = schema;
                config.Query = request?.Query;
                config.OperationName = request?.OperationName;
                config.Inputs = request?.Variables.ToInputs();
                config.UserContext = settings.BuildUserContext?.Invoke(context);

            });

            await WriteResponseAsync(context, result);
        }

        private async Task WriteResponseAsync(HttpContext context, ExecutionResult result)
        {
            var json = await writer.WriteToStringAsync(result);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = result.Errors?.Any() == true ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.OK;

            await context.Response.WriteAsync(json);
        }

        public static T Deserialize<T>(Stream s)
        {
            using (var reader = new StreamReader(s))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var ser = new JsonSerializer();
                return ser.Deserialize<T>(jsonReader);
            }
        }
    }
}