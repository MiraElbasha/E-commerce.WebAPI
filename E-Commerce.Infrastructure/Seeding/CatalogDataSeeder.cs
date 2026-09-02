using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Seeding
{
    internal class CatalogDataSeeder(StoreDbContext dbContext, ILogger<CatalogDataSeeder> logger) : IDataSeeder
    {
        public async Task SeedAsync(CancellationToken ct = default)
        {
            try
            {
                //check if there is any pending migrations
                var PendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(ct);
                //if there is any pending migrations --> add migrations
                if (PendingMigrations.Any())
                {
                    await dbContext.Database.MigrateAsync(ct);
                }
                //AppContext.BaseDirectory == G:\MiraElbasha-Route DiplomaC45\Back-End\Rana Hatem\Data Access Layer\API\E-Commerce\E-Commerce.API\bin\Debug\net8.0
                //AppContext.BaseDirectory + "DataSeed"
                var seedRoot = Path.Combine(AppContext.BaseDirectory, "DataSeed");
                await SeedIfEmptyAsync<ProductBrand>(seedRoot, "brands.json" , ct);
                await SeedIfEmptyAsync<ProductType>(seedRoot, "types.json" , ct);
                await SeedIfEmptyAsync<Product>(seedRoot, "products.json" , ct);
                await SeedIfEmptyAsync<DeliveryMethod>(seedRoot, "delivery.json" , ct);
                await dbContext.SaveChangesAsync(ct);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Catalog Data Seeding Failed");
                throw;
            }
        }

        //T == Entity
        private async Task SeedIfEmptyAsync<T>(string root,string FileName , CancellationToken ct = default) where T : class 
        {
            //Set<T> == DbSet<Entity>

            //if this entity has any records (rows) return (do not do anything of the following methods)
            //without this method the data will be seedede each time the application runs
            if (await dbContext.Set<T>().AnyAsync(ct)) return;
            //file root == Data/SeedData
            //filename == "products.json"
            //path = file root + filename  => Data/SeedData/products.json
            var path = Path.Combine(root, FileName);
            //if it's not
            if (!File.Exists(path))
            {
                //log this error in console
                logger.LogWarning($"Seed File Not Found {path}");
                //the out of the function
                return;
            }
            //if exits open it then read this file
            await using var stream = File.OpenRead(path);
            //read this json file then translate all it's data to C# Objects in a list<Entity>
            var items = await JsonSerializer.DeserializeAsync<List<T>>
                //ignore CaseSensitivitiy --> ID = id
                //take this JSON data from this stream to be translated to C# Objects
                // new JsonSerializerOptions { PropertyNameCaseInsensitive = true } ==> means this is the settings/options i want you to follow it while translating this JSON data to C# Objects -- ignore Case Sensitivity --
                (stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, ct);
            //if there is any objects translated
            if (items?.Count > 0)
            {
                //add them to the DbSet<Entity>
                await dbContext.Set<T>().AddRangeAsync(items, ct);
            }
        }
    }
}
