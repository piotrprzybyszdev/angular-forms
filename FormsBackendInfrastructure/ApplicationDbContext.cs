using FormsBackendCommon.Model;
using Microsoft.EntityFrameworkCore;

namespace FormsBackendInfrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }
    public DbSet<TaskModel> Tasks { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<TaskModel>().HasKey(e => e.Id);

        builder.Entity<TaskModel>().HasOne(e => e.User);

        base.OnModelCreating(builder);
    }
}
