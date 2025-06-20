using Microsoft.EntityFrameworkCore;
using ECommerce.API.Data;  // Đảm bảo bạn đã sử dụng đúng namespace cho DbContext
using ECommerce.API.Models;  // Đảm bảo bạn đã sử dụng đúng namespace cho các model như Product
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ DbContext để kết nối với cơ sở dữ liệu
builder.Services.AddDbContext<ECommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ECommerceDb")));

// Đăng ký các dịch vụ cho API controllers
builder.Services.AddControllers();

// Thêm Swagger để dễ dàng kiểm tra API trong môi trường phát triển
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECommerce API", Version = "v1" });
});

var app = builder.Build();

// Cấu hình Swagger UI cho môi trường phát triển
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce API v1");
        c.RoutePrefix = string.Empty;  // Swagger UI sẽ được hiển thị tại trang chủ (http://localhost:5000)
    });
}

// Cấu hình middleware cho HTTPS và các API controllers
app.UseHttpsRedirection();
app.MapControllers();  // Đảm bảo rằng các API controller được đăng ký và xử lý các yêu cầu

app.Run();
