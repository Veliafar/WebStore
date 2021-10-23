using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;

namespace WebStore.Data
{
    public class WebStoreDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<WebStoreDbInitializer> _Logger;

        public WebStoreDbInitializer(WebStoreDB db,
            ILogger<WebStoreDbInitializer> Logger
            )
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

            try
            {
                await InitializeProductsAsync();
            }
            catch (Exception e)
            {
                _Logger.LogError(e, "Product catalog init error");
                throw;
            }

            //try
            //{
            //    await InitializeIdentityAsync();
            //}
            //catch (Exception e)
            //{

            //    _Logger.LogError(e, "Identity system init error");
            //    throw;
            //}
        }

        private async Task InitializeProductsAsync()
        {

            if (_db.Sections.Any())
            {
                _Logger.LogInformation("**LOGGER** DB loaded without init -- DB Exist already");
                return;
            }

            _Logger.LogInformation("**LOGGER** Write Sections...");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Sections.AddRange(TestData.Sections);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF");

                await _db.Database.CommitTransactionAsync();
            }
            _Logger.LogInformation("**LOGGER** Write Sections success");


            _Logger.LogInformation("**LOGGER** Write Brands...");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Brands.AddRange(TestData.Brands);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF");

                await _db.Database.CommitTransactionAsync();
            }
            _Logger.LogInformation("**LOGGER** Write Brands success");


            _Logger.LogInformation("**LOGGER** Write Products...");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Products.AddRange(TestData.Products);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] OFF");

                await _db.Database.CommitTransactionAsync();
            }
            _Logger.LogInformation("**LOGGER** Write Products success");

        }

        //private async Task InitializeIdentityAsync()
        //{

        //}
    }
}
