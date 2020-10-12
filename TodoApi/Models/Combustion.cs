using System.Collections.Generic;
using System.Text.Json.Serialization;
using GraphQL.DataLoader;
using GraphQL.Types;
using TodoApi.Graphql;

namespace TodoApi.Models
{
    public class Combustion
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public List<Car> Cars { get; set; }
    }

    public class InputCombustionType : InputObjectGraphType<Combustion>
        {
            public InputCombustionType()
            {
                //this.AuthorizeWith("AdminPolicy");
                Field<NonNullGraphType<StringGraphType>>(nameof(Combustion.Name));
            }
        }
    public class CombustionGraphType : ObjectGraphType<Combustion>
    {

        public CombustionGraphType(IDataLoaderContextAccessor accessor, IGraphStore<Car> carStore, ApplicationDbContext _context)
        {
            Field(x => x.Id).Description("Id de la combustion del carro");
            Field(x => x.Name).Description("Tipo combustion del carro");

            Field<ListGraphType<CarGraphType>, IEnumerable<Car>>()
                .Name("Cars")
                .Description("Get all cars")
                .ResolveAsync(ctx =>
                {
                    var carsLoader = accessor.Context.GetOrAddCollectionBatchLoader<int?, Car>("GetCars",
                        (ids) => carStore.GetAllAsync(ids,"Combustion"));

                    return carsLoader.LoadAsync(ctx.Source.Id);
                });
        }
    }
}