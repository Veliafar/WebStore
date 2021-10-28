using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Data
{
    public class WebStoreDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;
        private readonly ILogger<WebStoreDbInitializer> _Logger;

        public WebStoreDbInitializer(
            WebStoreDB db,
            UserManager<User> UserManager,
            RoleManager<Role> RoleManager,
            ILogger<WebStoreDbInitializer> Logger)
        {
            _db = db;
            _UserManager = UserManager;
            _RoleManager = RoleManager;
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

            try
            {
                await InitializeIdentityAsync();
            }
            catch (Exception e)
            {

                _Logger.LogError(e, "Identity system init error");
                throw;
            }
        }

        private async Task InitializeProductsAsync()
        {


            var timer = Stopwatch.StartNew();
            if (_db.Sections.Any())
            {
                _Logger.LogInformation("**LOGGER** DB loaded without init -- DB Exist already");
                return;
            }

            _Logger.LogInformation("**LOGGER** Write Sections...");
            await using (await _db.Database.BeginTransactionAsync())

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
            _Logger.LogInformation("**LOGGER** Write data success for {0} ms", timer.Elapsed.TotalMilliseconds);

        }

        private async Task InitializeIdentityAsync()
        {
            _Logger.LogInformation("Identity system init");
            var timer = Stopwatch.StartNew();

            //if(!await _RoleManager.RoleExistsAsync(Role.Administrators))
            //{
            //    await _RoleManager.CreateAsync(new Role { Name = Role.Administrators });
            //}


            async Task CheckRole(string RoleName)
            {
                if (await _RoleManager.RoleExistsAsync(RoleName))
                {
                    _Logger.LogInformation("Role {0} is exist", RoleName);
                }
                else
                {
                    _Logger.LogInformation("Role {0} is Not exist", RoleName);
                    await _RoleManager.CreateAsync(new Role { Name = RoleName });
                    _Logger.LogInformation("Role {0} successfuly created", RoleName);
                }
            }

            await CheckRole(Role.Administrators);
            await CheckRole(Role.Users);

            if ( await _UserManager.FindByNameAsync(User.Administrator) is null)
            {
                _Logger.LogInformation("User {0} is Not exist", User.Administrator);

                var admin = new User
                {
                    UserName = User.Administrator
                };

                var creation_result = await _UserManager.CreateAsync(admin, User.DefaultAdminPass);
                if (creation_result.Succeeded)
                {
                    _Logger.LogInformation("User {0} successfuly created", User.Administrator);

                    await _UserManager.AddToRoleAsync(admin, Role.Administrators);

                    _Logger.LogInformation("User {0} successfuly get role {1}"
                        , User.Administrator
                        , Role.Administrators);
                } else
                {
                    var errors = creation_result.Errors.Select(err => err.Description).ToArray();
                    _Logger.LogInformation("Administrator account is Not created! Error: {0}"
                        , string.Join(", ", errors));

                    throw new InvalidOperationException($"Impossible to create Administator {string.Join(", ", errors)}");
                }

                _Logger.LogInformation("Identity system data added successfuly to DB for {0} ms", timer.Elapsed.TotalMilliseconds);
            }
        }
    }
}
