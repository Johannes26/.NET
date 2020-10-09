using System.Collections.Generic;
using System.Text.Json.Serialization;
using GraphQL.DataLoader;
using GraphQL.Types;
using TodoApi.Graphql;

namespace TodoApi.Models
{
    public class Brand
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public List<Car> Cars { get; set; }

    }
    public class BrandGraphType : ObjectGraphType<Brand>
    {

        public class InputBrandType : InputObjectGraphType<Brand>
        {
            public InputBrandType()
            {
                Field<NonNullGraphType<StringGraphType>>(nameof(Brand.Name));
            }
        }
        public BrandGraphType(IDataLoaderContextAccessor accessor, IGraphStore<Car> carStore, ApplicationDbContext _context)
        {
            Field(x => x.Id).Description("Id del modelo del carro");
            Field(x => x.Name).Description("Placa del modelo del carro");

            Field<ListGraphType<CarGraphType>, IEnumerable<Car>>()
                .Name("Cars")
                .Description("Get all cars")
                .ResolveAsync(ctx =>
                {
                    var carsLoader = accessor.Context.GetOrAddCollectionBatchLoader<int?, Car>("GetCars",
                        (ids) => carStore.GetAllAsync(ids));

                    return carsLoader.LoadAsync(ctx.Source.Id);
                });
        }
    }
}