# Bài Lab 2: Model Binding trong ASP.NET Core MVC

Đây là dự án thực hành Lab 2 cho môn NET201 - Lập Trình Web Nâng Cao. Dự án minh họa cách sử dụng **Model Binding** để ánh xạ dữ liệu từ HTTP Requests (Query String, Form Data) vào các đối tượng C# (Models).

## 📋 Tính Năng Chính

Dự án bao gồm các chức năng chính sau để minh họa các kỹ thuật Model Binding:

1.  **Tìm Kiếm Sản Phẩm (Product Search)**
    *   **Kỹ thuật**: Sử dụng `[FromQuery]` (hoặc mặc định) để lấy tham số tìm kiếm từ URL.
    *   **Mô tả**: Cho phép tìm kiếm sản phẩm theo tên và khoảng giá (MinPrice, MaxPrice).

2.  **Tạo Đơn Hàng (Create Order)**
    *   **Kỹ thuật**: Sử dụng `[FromForm]` để bind dữ liệu từ form HTML phức tạp.
    *   **Điểm nổi bật**:
        *   Bind đối tượng `Order` chứa danh sách `OrderDetails`.
        *   Sử dụng JavaScript để thêm/xóa dòng chi tiết đơn hàng động.
        *   ASP.NET Core tự động map các input có name dạng `OrderDetails[0].ProductName` vào List.

3.  **Lọc Đơn Hàng (Filter Orders)**
    *   **Kỹ thuật**: Bind dữ liệu vào một object `OrderFilterModel` từ Query String.
    *   **Mô tả**: Xem danh sách đơn hàng và lọc theo ngày tháng, trạng thái.

## 🛠️ Cài Đặt và Chạy Dự Án

### Yêu Cầu
*   .NET SDK (phiên bản 6.0 trở lên)
*   SQL Server (hoặc LocalDB)

### Các Bước Thực Hiện

1.  **Cấu hình Database**
    *   Mở file `appsettings.json` và kiểm tra chuỗi kết nối `DefaultConnection`.
    *   Mặc định đang trỏ tới `(localdb)\\mssqllocaldb`.

2.  **Khởi Tạo Cơ Sở Dữ Liệu (Migration)**
    Dự án đã tích hợp sẵn cơ chế **Seed Data** (dữ liệu mẫu) khi khởi tạo database.
    Mở terminal tại thư mục dự án (`Lab2ModelBinding`) và chạy lệnh:

    ```bash
    dotnet ef database update
    ```
    *Lệnh này sẽ tạo database và tự động thêm các sản phẩm và đơn hàng mẫu.*

3.  **Chạy Ứng Dụng**
    ```bash
    dotnet run
    ```
    Truy cập địa chỉ `http://localhost:5xxx` (hoặc `https://localhost:7xxx`) được hiển thị trên màn hình.

## 📂 Cấu Trúc Dự Án

*   **Controllers**
    *   `ProductsController.cs`: Xử lý tìm kiếm sản phẩm.
    *   `OrdersController.cs`: Xử lý tạo và lọc đơn hàng.
*   **Models**
    *   `Product`, `Order`, `OrderDetail`: Các Entity Framework Models.
    *   `ProductSearchModel`, `OrderFilterModel`: Các ViewModels dùng để hứng dữ liệu tìm kiếm/lọc.
    *   `AppDbContext`: Cấu hình database và Seed Data trong `OnModelCreating`.
*   **Views**
    *   `Products/Search.cshtml`: Giao diện tìm kiếm.
    *   `Orders/Create.cshtml`: Giao diện tạo đơn hàng với bảng nhập liệu động.
    *   `Orders/Filter.cshtml`: Giao diện danh sách và lọc đơn hàng.

## 📝 Lưu Ý
*   Dữ liệu mẫu (`Seed Data`) được định nghĩa trong `AppDbContext.cs`.
*   Để reset database, bạn có thể xóa database cũ và chạy lại lệnh `dotnet ef database update`.
