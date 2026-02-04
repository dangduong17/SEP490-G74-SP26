# SEP490-G74-RJMS - Recruitment Job Management System

## Mô tả dự án

Hệ thống quản lý tuyển dụng (RJMS) được phát triển bằng ASP.NET Core MVC với các tính năng chính:

### Các tính năng chính

#### FF-01: Authentication & Authorization

- Đăng nhập hệ thống
- Đăng xuất khỏi hệ thống
- Kiểm soát truy cập dựa trên vai trò (Candidate / Recruiter / Admin)

#### FF-02: Candidate Management

- Xem danh sách ứng viên
- Xem chi tiết hồ sơ ứng viên
- Chỉnh sửa thông tin ứng viên
- Cập nhật trạng thái tài khoản ứng viên (active / inactive)

#### FF-03: CV Management

- Xem danh sách CV
- Tạo CV mới
- Xem chi tiết CV
- Chỉnh sửa nội dung CV
- Đặt CV mặc định cho việc ứng tuyển

#### FF-04: Job Management

- Xem danh sách các tin tuyển dụng
- Tạo tin tuyển dụng mới
- Xem chi tiết công việc

## Cấu trúc dự án

```
SEP490-G74-RJMS/
├── Controllers/
│   ├── HomeController.cs
│   ├── CandidatesController.cs
│   ├── CVsController.cs
│   └── JobsController.cs
├── Models/
│   ├── ApplicationUser.cs
│   ├── Candidate.cs
│   ├── CV.cs
│   ├── Job.cs
│   ├── JobApplication.cs
│   ├── ErrorViewModel.cs
│   └── PaginatedList.cs
├── Views/
│   ├── Home/
│   ├── Candidates/
│   ├── CVs/
│   ├── Jobs/
│   └── Shared/
├── Data/
│   ├── ApplicationDbContext.cs
│   └── SeedData.cs
└── wwwroot/
    ├── css/
    ├── js/
    └── uploads/
```

## Công nghệ sử dụng

- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server (Entity Framework Core)
- **Authentication**: ASP.NET Core Identity
- **UI Framework**: Bootstrap 5
- **Architecture**: Model-View-Controller (MVC)

## Cách chạy dự án

1. Cài đặt .NET 8.0 SDK
2. Restore packages:
   ```bash
   dotnet restore
   ```
3. Cập nhật database:
   ```bash
   dotnet ef database update
   ```
4. Chạy ứng dụng:
   ```bash
   dotnet run
   ```

## Tài khoản mặc định

- **Email**: admin@rjms.com
- **Password**: Admin@123456
- **Role**: Admin

## Vai trò người dùng

- **Admin**: Toàn quyền quản lý hệ thống
- **Recruiter**: Quản lý tin tuyển dụng và xem ứng viên
- **Candidate**: Tạo CV và ứng tuyển công việc
