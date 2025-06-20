using ECommerce.API.Data;
using ECommerce.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorysController : ControllerBase
    {
        private readonly ECommerceContext _context;

        public CategorysController(ECommerceContext context)
        {
            _context = context;
        }

        // POST: api/Categorys
        [HttpPost]
        public async Task<ActionResult<Product>> PostCategories(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Tạo danh mục thành công!" });
        }

        // GET: api/Categorys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            // Trả về danh sách tất cả các danh mục
            return await _context.Categories.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            // Tìm danh mục có ID tương ứng
            var category = await _context.Categories.FindAsync(id);

            // Nếu không tìm thấy danh mục, trả về mã lỗi 404
            if (category == null) return NotFound();

            // Trả về danh mục nếu tìm thấy
            return category;
        }
        // PUT: api/Categorys/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            // Kiểm tra xem ID trong URL có khớp với ID của danh mục không
            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null)
            {
                return NotFound(new { message = "Danh mục không tồn tại!" });
            }
            
            // Cập nhật thông tin danh mục
            existingCategory.Name = category.Name;

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật danh mục thành công!" });
        }
    }
}
