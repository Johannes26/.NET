using GraphQL;
using GraphQL.Types;
using TodoApi.Models;
using static TodoApi.Models.BrandGraphType;

namespace TodoApi.Graphql
{
    public class AppMutation : ObjectGraphType
    {
        public readonly ApplicationDbContext _context;
        public AppMutation(ApplicationDbContext applicationDbContext,IGraphStore<Brand> brandStore)
        {
            _context = applicationDbContext;
            Field<BrandGraphType>(
                "create_brand",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<InputBrandType>> { Name = "brand" }),
                resolve: context =>
                {
                    var brand = context.GetArgument<Brand>("brand");
                    return brandStore.CreateAsync(brand);
                }
            );

            Field<BrandGraphType>(
                "update_brand",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" },
                    new QueryArgument<NonNullGraphType<InputBrandType>> { Name = "brand" }
                    ),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    var brand = context.GetArgument<Brand>("brand");
                    return brandStore.UpdateAsync(id,brand);
                }
            );
        }

    }
}