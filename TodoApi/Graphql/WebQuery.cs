using System.Collections.Generic;
using System.Linq;
using GraphQL;
using GraphQL.Types;
using TodoApi.Models;

namespace TodoApi.Graphql
{
    public class WebQuery : ObjectGraphType
    {
        public readonly ApplicationDbContext _context;
        public WebQuery(ApplicationDbContext mcontext)
        {
            _context=mcontext;
            Field<ListGraphType<CarGraphType>>(
                "car_list",
                arguments: new QueryArguments
                (
                    new QueryArgument<IntGraphType>
                    {
                        Name = "modelo",
                        Description = "modelo del carro"
                    },
                    new QueryArgument<IntGraphType>
                    {
                        Name = "take",
                        Description = "records"
                    }
                ),
                resolve: context =>
                {
                    IQueryable<Car> query = _context.Cars;
                    var modelo = context.GetArgument<int?>("modelo");
                    var take = context.GetArgument<int?>("take");
                    if (modelo != null)
                    {
                        query = query.Where(x => x.Modelo == modelo);
                    }
                    if (take != null)
                    {
                        query = query.Take(take.Value);
                    }
                    return query.ToList();
                }
            );

            Field<CarGraphType>(
                "car",
                arguments: new QueryArguments
                {
                    new QueryArgument<NonNullGraphType<IntGraphType>>
                    {
                        Name = "id",
                        Description ="Id del modelo del carro"
                    }
                },
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    return _context.Cars
                    .SingleOrDefault(x => x.Id == id);
                }

            );

            Field<ListGraphType<BrandGraphType>>(
                "brand_list",
                arguments: new QueryArguments
                {
                    new QueryArgument<NonNullGraphType<IntGraphType>>
                    {
                        Name = "id",
                        Description ="id del carro"
                    }
                },
                resolve: context =>
                {
                    IQueryable<Brand> query = _context.Brands;
                    var id = context.GetArgument<int?>("id");
                    var take = context.GetArgument<int?>("take");
                    if (id != null)
                    {
                        query = query.Where(x => x.Id == id);
                    }
                    if (take != null)
                    {
                        query = query.Take(take.Value);
                    }
                    return query.ToList();
                }

            );

            Field<ListGraphType<BrandGraphType>>(
                "brand",
                resolve: context =>
                {
                    return _context.Brands.ToList();
                }
            );

            Field<ListGraphType<CombustionGraphType>>(
                "combustion_list",
                arguments: new QueryArguments
                {
                    new QueryArgument<IntGraphType>
                    {
                        Name = "id",
                        Description ="id del carro"
                    }
                },
                resolve: context =>
                {
                    IQueryable<Combustion> query = _context.Combustions;
                    var id = context.GetArgument<int?>("id");
                    var take = context.GetArgument<int?>("take");
                    if (id != null)
                    {
                        query = query.Where(x => x.Id == id);
                    }
                    if (take != null)
                    {
                        query = query.Take(take.Value);
                    }
                    return query.ToList();
                }

            );

            


        }
    }
}