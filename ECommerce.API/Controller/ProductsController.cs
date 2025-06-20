using ECommerce.API.Data;
using ECommerce.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ECommerceContext _context;

        public ProductsController(ECommerceContext context)
        {
            _context = context;
        }
        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Tạo sản phẩm thành công!" });
        }
        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            // Trả về danh sách tất cả các sản phẩm
            return await _context.Products.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return product;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound(new { message = "Sản phẩm không tồn tại!" });
            }
            // Cập nhật thông tin sản phẩm
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.CategoryId = product.CategoryId;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật sản phẩm thành công!" });

        }
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProducts(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(new { message = "Tên sản phẩm không được để trống!" });
            }

            var products = await _context.Products
                              .Where(p => p.Name != null && EF.Functions.Like(p.Name, $"%{name}%"))
                              .ToListAsync();
            
            if (products.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm nào!" });
            }

            return Ok(products);
        }
    }
}
