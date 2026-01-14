# 1. Dùng hình ảnh SDK để Build (Biên dịch code)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy file dự án và tải thư viện
COPY *.csproj ./
RUN dotnet restore

# Copy toàn bộ code và build ra file chạy (Release)
COPY . ./
RUN dotnet publish -c Release -o out

# 2. Dùng hình ảnh ASP.NET để Chạy (Nhẹ hơn)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Cấu hình cổng (Render dùng cổng 10000)
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

# Chạy file DLL (Tên file phải khớp với tên dự án của bạn)
ENTRYPOINT ["dotnet", "QuanLyThuVienSo.API.dll"]