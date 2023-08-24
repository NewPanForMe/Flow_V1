using BMS_Models.DbModels;
namespace BMS_Db.EfContext;
using Microsoft.EntityFrameworkCore;
public class BmsV1DbContext : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<Module> Module { get; set; }
    public DbSet<NLog> NLog { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<UserRole> UserRole { get; set; }
    public DbSet<MenuRole> MenuRole { get; set; }
    public DbSet<SmsLog> SmsLog { get; set; }
    public DbSet<FileUpload> FileUpload { get; set; }
    public DbSet<Bill> Bill { get; set; }
    public DbSet<BillDetail> BillDetail { get; set; }

    public BmsV1DbContext(DbContextOptions<BmsV1DbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}