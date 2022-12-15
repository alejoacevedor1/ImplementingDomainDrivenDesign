using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using API.DTOs;
using API.Services.Helper;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Domain.Entities.Product;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    /// <summary>
    /// Services available for products.
    /// </summary>
	public class ProductService
    {        
        #region Dependency Injection
        /// <summary>
        /// InventoryContext dependency injection.
        /// </summary>
        private readonly InventoryContext _inventoryContext;
        #endregion


        #region Contructor
        /// <summary>
        /// Contructor with dependency injection.
        /// </summary>
        /// <param name="inventoryContext">InventoryContext depndency injection</param>
        public ProductService(InventoryContext inventoryContext)
		{
            _inventoryContext = inventoryContext;
		}
        #endregion


        #region Public Methods
        /// <summary>
        /// Get all products by applying filters.
        /// </summary>
        /// <param name="filters">Filter list by entity property</param>
        /// <returns>ProductInfoDTO list</returns>
        public List<ProductInfoDTO> GetAllProducts(PaginationFilter paginationFilter)
		{
            Expression<Func<Product, bool>> expression = BuilderConditionHelper.EvaluateFilters<Product>(paginationFilter.FilterList);

            IQueryable<Product> products = _inventoryContext.Products.Include(i => i.Provider)
                                                                     .Where(expression)
                                                                     .Skip((paginationFilter.CurrentPage -1) * paginationFilter.ItemsPerPage)
                                                                     .Take(paginationFilter.ItemsPerPage).AsQueryable();

            List<ProductInfoDTO> productInfoDTOList = MapperProductToDTO(products) as List<ProductInfoDTO>;

            return productInfoDTOList;
        }

        /// <summary>
        /// Gets the product by id.
        /// </summary>
        /// <param name="id">Product identifier to consult</param>
        /// <returns>ProductInfoDTO</returns>
        public ProductInfoDTO GetProductById(int id)
        {
            ProductInfoDTO productInfoDTO = new();

            Product product = _inventoryContext.Products.Include(i => i.Provider).FirstOrDefault(f => f.IdProduct.Equals(id));

            if (product != null)
                productInfoDTO = MapperProductToDTO(product) as ProductInfoDTO;

            return productInfoDTO;
        }

        /// <summary>
        /// Add or update product.
        /// </summary>
        /// <param name="addProduct">Product information</param>
        /// <returns>Product added with your Id or updated product</returns>
        public ProductInfoDTO AddOrUpdateProduct(AddProduct addProduct)
        {
            ValidateManufactureDate(addProduct);

            Product product = MapperProductToDTO(addProduct) as Product;

            product = PurgeRelationalProperties(product);

            _inventoryContext.Update(product);
            _inventoryContext.SaveChanges();

            ProductInfoDTO productInfoDTO = GetProductById(product.IdProduct);
            
            return productInfoDTO;
        }


        public ProductInfoDTO DeleteProduct(int id)
        {
            Product product = _inventoryContext.Products.FirstOrDefault(x => x.IdProduct.Equals(id));

            if (product != null)
            {
                product.Active = false;

                _inventoryContext.Entry(product).Property("Active").IsModified = true;
                _inventoryContext.SaveChanges();
            }

            ProductInfoDTO productInfoDTO = GetProductById(id);

            return productInfoDTO;
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Mapping of the original information to the response DTO.
        /// </summary>
        /// <param name="info"></param>
        /// <returns>Products mapped to the DTO</returns>
        private static object MapperProductToDTO(object info)
        {
            MapperConfiguration confAutoMapper = new(cfg =>
            {
                cfg.AddCollectionMappers();

                cfg.CreateMap<Product, ProductInfoDTO>()
                   .ForMember(dto => dto.DesciptionProvider, o => o.MapFrom(src => src.Provider.Description))
                   .ForMember(dto => dto.PhoneProvider, o => o.MapFrom(src => src.Provider.Phone))
                   .EqualityComparison((o, dto) => o.IdProduct == dto.IdProduct);

                cfg.CreateMap<AddProduct, Product>()
                   .EqualityComparison((o, dto) => o.IdProduct == dto.IdProduct);
            }
            );

            Mapper mapper = new(confAutoMapper);

            if (info is IQueryable<Product>)
            {
                List<ProductInfoDTO> productInfoDTOList = mapper.Map<List<ProductInfoDTO>>(info as IQueryable<Product>);
                return productInfoDTOList;
            }
            else if (info is Product)            
            {
                ProductInfoDTO productInfoDTO = mapper.Map<ProductInfoDTO>(info as Product);
                return productInfoDTO;
            }
            else
            {
                Product product = mapper.Map<Product>(info as AddProduct);
                return product;
            }
        }

        /// <summary>
        /// Verify that the manufacturing date is not greater than the expiration date to allow saving the new product.
        /// </summary>
        /// <param name="addProduct">Product information to add</param>
        /// <exception cref="Exception">Exception if the manufacturing date is greater than the expiration date</exception>
        private static void ValidateManufactureDate(AddProduct addProduct)
        {
            if (addProduct.ManufacturingDate >= addProduct.ValidityDate)
                throw new Exception("Error: The manufacturing date cannot be greater than the expiration date");
        }

        /// <summary>
        /// The property information with related information is nulled so as not to modify it by mistake when saving or updating a product.
        /// </summary>
        /// <param name="product">Product to save and/or update</param>
        /// <returns>Product with related properties set to null</returns>
        private static Product PurgeRelationalProperties(Product product)
        {
            product.Provider = null;

            return product;
        }        
        #endregion
    }
}

