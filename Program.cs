using Microsoft.EntityFrameworkCore;
using QuanLyThuVienSo.API.Models;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CẤU HÌNH CORS (QUAN TRỌNG CHO FRONTEND) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()  // Cho phép mọi nơi gọi vào
               .AllowAnyMethod()  // Cho phép mọi method (GET, POST...)
               .AllowAnyHeader(); // Cho phép mọi Header
    });
});

// --- 2. ĐĂNG KÝ DỊCH VỤ (SERVICES) ---

builder.Services.AddControllers();

// Đăng ký Database
// Lưu ý: Lên Render chúng ta sẽ dùng Biến Môi Trường để thay thế chuỗi kết nối này
builder.Services.AddDbContext<QuanLyThuVienSoContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Đăng ký DAL
builder.Services.AddScoped<QuanLyThuVienSo.API.DAL.TaiKhoanDAL>();
builder.Services.AddScoped<QuanLyThuVienSo.API.DAL.SachDAL>();
builder.Services.AddScoped<QuanLyThuVienSo.API.DAL.DocGiaDAL>();
builder.Services.AddScoped<QuanLyThuVienSo.API.DAL.PhieuMuonDAL>();
builder.Services.AddScoped<QuanLyThuVienSo.API.DAL.TacGiaDAL>();

// Đăng ký BUS
builder.Services.AddScoped<QuanLyThuVienSo.API.BUS.TaiKhoanBUS>();
builder.Services.AddScoped<QuanLyThuVienSo.API.BUS.SachBUS>();
builder.Services.AddScoped<QuanLyThuVienSo.API.BUS.DocGiaBUS>();
builder.Services.AddScoped<QuanLyThuVienSo.API.BUS.PhieuMuonBUS>();
builder.Services.AddScoped<QuanLyThuVienSo.API.BUS.TacGiaBUS>();

var app = builder.Build();

// --- 3. CẤU HÌNH PIPELINE ---

// Kích hoạt CORS (Phải đặt trước Auth và MapControllers)
app.UseCors("AllowAll");

// QUAN TRỌNG: Bỏ 'if (IsDevelopment)' để Swagger hiện luôn trên Render
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); 

// Render thường dùng PORT do biến môi trường cung cấp, code này tự động nhận diện
app.Run();