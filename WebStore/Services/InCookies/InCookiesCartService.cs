using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Services.InCookies
{
    public class InCookiesCartService : ICartService
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IProductData _ProductData;
        private readonly String _CartName;

        public InCookiesCartService(IHttpContextAccessor HttpContextAccessor, IProductData ProductData)
        {
            _HttpContextAccessor = HttpContextAccessor;
            _ProductData = ProductData;

            var user = _HttpContextAccessor.HttpContext.User;
            var user_name = user.Identity.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _CartName = $"WebStore.Cart{user_name}";
        }

        public void Add(int Id)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Decrement(int Id)
        {
            throw new NotImplementedException();
        }

        public CartViewModel GetViewModel()
        {
            throw new NotImplementedException();
        }

        public void Remove(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
