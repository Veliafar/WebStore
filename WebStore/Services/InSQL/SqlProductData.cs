using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreDB _db;

        public SqlProductData(WebStoreDB db) => _db = db;

        public IEnumerable<Section> GetSections() => _db.Sections;

        public Section GetSectionById(int Id) => _db.Sections.SingleOrDefault(section => section.Id == Id); 

        public IEnumerable<Brand> GetBrands() => _db.Brands;

        public Brand GetBrandById(int Id) => _db.Brands.Find(Id);

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Product> query = _db.Products
                .Include(product => product.Brand)
                .Include(product => product.Section);

            //if (Filter?.SectionId != null)
            //    query = query.Where(product => product.SectionId == Filter.SectionId);            

            if(Filter?.Ids.Length > 0)
            {
                query = query.Where(product => Filter.Ids.Contains(product.Id));
            } 
            else
            {
                if (Filter?.SectionId is { } section_id)
                    query = query.Where(product => product.SectionId == section_id);

                if (Filter?.BrandId is { } brand_id)
                    query = query.Where(product => product.SectionId == brand_id);
            }

            return query;
        }

        public Product GetProductById(int Id) => _db.Products
            .Include(product => product.Brand)
            .Include(product => product.Section)
            .FirstOrDefault(product => product.Id == Id);
    }
}
