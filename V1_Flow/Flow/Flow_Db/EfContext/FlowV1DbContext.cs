namespace Flow_Db.EfContext;
using Microsoft.EntityFrameworkCore;
public class FlowV1DbContext : DbContext
{

    public FlowV1DbContext(DbContextOptions<FlowV1DbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}