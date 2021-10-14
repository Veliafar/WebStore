using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.Entities.Identity
{
    class Role : IdentityRole
    {
        public const string Administrators = "Administrators";

        public const string Users = "Users";
    }
}
