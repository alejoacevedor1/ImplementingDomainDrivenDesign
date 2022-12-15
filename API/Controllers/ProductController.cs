using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Services;
using Domain.Entities.Product;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller to access products.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        /// <summary>
        /// ProductService dependency injection.
        /// </summary>
        private readonly ProductService _productService;

        /// <summary>
        /// Contructor with dependency injection.
        /// </summary>
        /// <param name="productService">ProductService dependency injection</param>
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all products by applying filters.
        /// </summary>
        /// <param name="filters">Filter list by entity property</param>
        /// <returns>filtered products with information of the corresponding DTO</returns>
        [HttpGet]
        [Route("/GetAllProducts")]
        public IActionResult GetAllProducts([FromBody] PaginationFilter paginationFilter)
        {
            List<ProductInfoDTO> products = _productService.GetAllProducts(paginationFilter);

            return Ok(products);
        }

        /// <summary>
        /// Get the product by the requested id.
        /// </summary>
        /// <param name="id">Product identifier to consult</param>
        /// <returns>Product with the id indicated in the request</returns>
        [HttpGet]
        [Route("/GetProductById/{id}")]
        public IActionResult GetProductById(int id)
        {
            ProductInfoDTO productInfoDTO = _productService.GetProductById(id);

            return Ok(productInfoDTO);
        }

        /// <summary>
        /// Add a new product.
        /// </summary>
        /// <param name="addProduct">Product information to add</param>
        /// <returns>Product added with your Id</returns>
        [HttpPost]
        [Route("/AddProduct")]
        public IActionResult AddProduct([FromBody] AddProduct addProduct)
        {
            ProductInfoDTO productInfoDTO = _productService.AddOrUpdateProduct(addProduct);

            return Ok(productInfoDTO);
        }

        /// <summary>
        /// Update a exist product.
        /// </summary>
        /// <param name="addProduct"><Product information to update/param>
        /// <returns>Updated product</returns>
        [HttpPut]
        [Route("/UpdateProduct")]
        public IActionResult UpdateProduct([FromBody] AddProduct addProduct)
        {
            ProductInfoDTO productInfoDTO = _productService.AddOrUpdateProduct(addProduct);

            return Ok(productInfoDTO);
        }

        /// <summary>
        /// Changes the product status to inactive
        /// </summary>
        /// <param name="id">Product identifier to inactivate</param>
        /// <returns>Inactive product</returns>
        [HttpDelete]
        [Route("/DeleteProduct/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            ProductInfoDTO productInfoDTO = _productService.DeleteProduct(id);

            return Ok(productInfoDTO);
        }
    }
}