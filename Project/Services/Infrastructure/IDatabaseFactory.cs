namespace Services.Infrastructure
{
    public interface IDatabaseFactory
    {
        ApplicationDbContext Get();
     
    }
}