using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Components
{
    //[ViewComponent(Name = "BrandsView")]
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BrandsViewComponent(IProductData ProductData)
        {
            _ProductData = ProductData;
        }

        //public async Task<IViewComponentResult> InvokeAsync() => View();
        public IViewComponentResult Invoke() => View(GetBrands());

        private IEnumerable<BrandViewModel> GetBrands()
        {
           return _ProductData.GetBrands()
                .OrderBy(brand => brand.Order)
                .Select(brand => new BrandViewModel 
                {
                    Id = brand.Id,
                    Name = brand.Name,
                });
        } 


    }
}
