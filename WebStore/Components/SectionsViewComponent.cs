using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Components
{
    public class SectionsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public SectionsViewComponent(IProductData ProductData)
        {
            _ProductData = ProductData;
        }

        public IViewComponentResult Invoke() 
        {
            var sections = _ProductData.GetSections();

            var parent_sections = sections.Where(section => section.ParentId is null);

            var parent_sections_views = parent_sections
                .Select(section => new SectionViewModel
                {
                    Id = section.Id,
                    Name = section.Name,
                    Order = section.Order,
                })
                .ToList();

            foreach(var parent_section in parent_sections_views)
            {
                var childs = sections.Where(section => section.Id == parent_section.Id);

                foreach (var child_section in childs)
                    parent_section.ChildSections.Add(new SectionViewModel 
                    {
                        Id = child_section.Id,
                        Name = child_section.Name,
                        Order = child_section.Order,
                        Parent = parent_section,
                    });

                parent_section.ChildSections.Sort((a,b)=> Comparer<int>.Default.Compare(a.Order, b.Order));
            }

            parent_sections_views.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));

            return View(parent_sections_views);
        }  
    }
}
