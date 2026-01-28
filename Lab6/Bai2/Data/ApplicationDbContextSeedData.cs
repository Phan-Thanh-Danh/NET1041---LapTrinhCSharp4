using Bai2.Models;

namespace Bai2.Data
{
    public static class ApplicationDbContextSeedData
    {
        public static void Seed(ApplicationDbContext context)
        {
            // CHỈ seed khi bảng rỗng
            if (context.Users.Any())
            {
                Console.WriteLine("--> Dữ liệu đã tồn tại. Bỏ qua Seed Data.");
                return;
            }

            Console.WriteLine("--> Database rỗng. Đang thêm Seed Data...");

            var users = new List<User>
            {
                new User { Name = "Nguyễn Văn A", Email = "a@gmail.com" },
                new User { Name = "Trần Văn B", Email = "b@gmail.com" },
                new User { Name = "Lê Văn C", Email = "c@gmail.com" },
                new User { Name = "Phạm Văn D", Email = "d@gmail.com" },
                new User { Name = "Hoàng Văn E", Email = "e@gmail.com" },
                new User { Name = "Đỗ Văn F", Email = "f@gmail.com" },
                new User { Name = "Bùi Văn G", Email = "g@gmail.com" },
                new User { Name = "Vũ Văn H", Email = "h@gmail.com" },
                new User { Name = "Ngô Văn I", Email = "i@gmail.com" },
                new User { Name = "Đặng Văn K", Email = "k@gmail.com" },
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
