using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;

namespace WebStore.Data
{
    public class WebStoreDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<WebStoreDbInitializer> _Logger;

        public WebStoreDbInitializer(WebStoreDB db, ILogger<WebStoreDbInitializer> Logger)
        {
            _db = db;
            _Logger = Logger;
        }

        public async Task InitializeAsync()
        {

            _Logger.LogInformation("**LOGGER** Launch DB init");

            //var db_deleted = await _db.Database.EnsureDeletedAsync();
            //var db_created = await _db.Database.EnsureCreatedAsync();

            var pending_migrations = await _db.Database.GetPendingMigrationsAsync();

            var applied_migrations = await _db.Database.GetAppliedMigrationsAsync();

            if (pending_migrations.Any())
            {
                _Logger.LogInformation("**LOGGER** use migrations {0}", string.Join(",", pending_migrations));
                await _db.Database.MigrateAsync();
            }

            await InitializeProductsAsync();
        }

        private async Task InitializeProductsAsync()
        {

            if (_db.Sections.Any())
            {
                _Logger.LogInformation("**LOGGER** DB loaded without init -- DB Exist already");
                return;
            }

            var sections_pool = TestData.Sections.ToDictionary(section => section.Id);
            var brands_pool = TestData.Brands.ToDictionary(brand => brand.Id);

            foreach (var child_section in TestData.Sections.Where(section => section.ParentId is not null))
                child_section.Parent = sections_pool[(int)child_section.ParentId!];

            foreach (var product in TestData.Products)
            {
                product.Section = sections_pool[product.SectionId];
                if (product.BrandId is { } brand_id)
                    product.Brand = brands_pool[brand_id];

                product.Id = 0;
                product.SectionId = 0;
                product.BrandId = null;
            }

            foreach (var section in TestData.Sections)
            {
                section.Id = 0;
                section.ParentId = null;
            }

            foreach (var brand in TestData.Brands)
                brand.Id = 0;


            _Logger.LogInformation("**LOGGER** Write data...");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Sections.AddRange(TestData.Sections);
                _db.Brands.AddRange(TestData.Brands);
                _db.Products.AddRange(TestData.Products);

                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
            }
            _Logger.LogInformation("**LOGGER** Write data success");

        }


    }
}
