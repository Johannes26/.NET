using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using GraphQL.DataLoader;
using GraphQL.Types;
using TodoApi.Graphql;

namespace TodoApi.Models
{
    public class Car
    {
        public int Id { get; set; }

        public string Placa { get; set; }

        public int Modelo { get; set; }

        public int numeroPuertas { get; set; }

        public int? BrandKey { get; set; }

        [ForeignKey("BrandKey")]
        public Brand Brand { get; set; }
    }

    public class CarGraphType : ObjectGraphType<Car>
    {
        public CarGraphType(IDataLoaderContextAccessor accessor, IGraphStore<Brand> brandStore)
        {
            Field(x => x.Id).Description("Id del modelo del carro");
            Field(x => x.Placa).Description("Placa del modelo del carro");
            Field(x => x.numeroPuertas).Description("numeroPuertas del modelo del carro");
            Field(x => x.Modelo).Description("Modelo del modelo del carro");

            Field<BrandGraphType,Brand>()
                .Name("Brand")
                .ResolveAsync(context =>
                {
                    var loader = accessor.Context.GetOrAddBatchLoader<int?, Brand>("GetUsersById",
                        ids => brandStore.GetUsersByIdAsync(ids, CancellationToken.None));

                    return loader.LoadAsync(context.Source.BrandKey);
                });
        }
    }


}