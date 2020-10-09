using System;
using GraphQL.Types;
using GraphQL.Utilities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TodoApi.Graphql
{
    public class AppSchema: Schema
    {
        public AppSchema(IServiceProvider services): base(services)
        {
            Query = services.GetRequiredService<WebQuery>();
            Mutation= services.GetRequiredService<AppMutation>();
        }
    }
}