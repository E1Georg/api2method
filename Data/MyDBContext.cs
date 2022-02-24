using Microsoft.EntityFrameworkCore;
#nullable disable


namespace API_T2.Data
{
    public partial class MyDBContext : DbContext
    {
        public MyDBContext()
        {
           Database.EnsureDeleted(); // UNIT TESTS
           Database.EnsureCreated(); // При проведении тестирования необходимо закомментировать эти строки
        }

        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {
            
        }

        public DbSet<API_T2.Models.Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=myServer; Database=myDataBase;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
