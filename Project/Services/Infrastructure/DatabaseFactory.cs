using Common;


namespace Services.Infrastructure
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private ApplicationDbContext _dataContext;

        public ApplicationDbContext Get()
        {
            _dataContext = _dataContext ?? (_dataContext = new ApplicationDbContext());

            _dataContext.Database.Log = log => Log.Write("EF", log);

            return _dataContext;
        }


        //日志处理

    }
}