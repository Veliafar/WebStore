using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Infrastucture.Mapping;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Services.InCookies
{
    public class InCookiesCartService : ICartService
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IProductData _ProductData;
        private readonly String _CartName;

        private Cart Cart
        {
            get
            {
                var context = _HttpContextAccessor.HttpContext;
                var cookies = context!.Response.Cookies;

                var cart_cookies = context.Request.Cookies[_CartName];
                if (cart_cookies is null)
                {
                    var cart = new Cart();
                    cookies.Append(_CartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }

                ReplaceCart(cookies, cart_cookies);
                return JsonConvert.DeserializeObject<Cart>(cart_cookies);
            }

            set => ReplaceCart(_HttpContextAccessor.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value));
        }

        private void ReplaceCart(IResponseCookies cookies, string cart)
        {
            cookies.Delete(_CartName);
            cookies.Append(_CartName, cart);
        }

        public InCookiesCartService(IHttpContextAccessor HttpContextAccessor, IProductData ProductData)
        {
            _HttpContextAccessor = HttpContextAccessor;
            _ProductData = ProductData;

            var user = _HttpContextAccessor.HttpContext.User;
            var user_name = user.Identity.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _CartName = $"KDVWebStore.Cart{user_name}";
        }

        public void Add(int Id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(item => item.ProductId == Id);
            if (item is null)
                cart.Items.Add(new CartItem
                {
                    ProductId = Id,
                    Quantity = 1
                });
            else
                item.Quantity++;

            Cart = cart;
        }

        public void Decrement(int Id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(item => item.ProductId == Id);

            if (item is null) return;

            if (item.Quantity > 0)
                item.Quantity--;

            if (item.Quantity <= 0)
                cart.Items.Remove(item);

            Cart = cart;
        }

        public void Remove(int Id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(item => item.ProductId == Id);

            if (item is null) return;

            cart.Items.Remove(item);

            Cart = cart;
        }


        public void Clear()
        {
            var cart = Cart;

            cart.Items.Clear();

            Cart = cart;
        }


        public CartViewModel GetViewModel()
        {
            var products = _ProductData.GetProducts(new ProductFilter()
            {
                Ids = Cart.Items.Select(item => item.ProductId).ToArray()
            });

            var products_views = products.ToView().ToDictionary(product => product.Id);

            return new CartViewModel
            {
                Items = Cart.Items
                    .Where(item => products_views.ContainsKey(item.ProductId))
                    .Select(item => (products_views[item.ProductId], item.Quantity))
            };
        }
    }
}
