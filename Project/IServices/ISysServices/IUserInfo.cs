namespace IServices.ISysServices
{
    public interface IUserInfo 
    {
        string EnterpriseId { get; }

        string UserId { get; }

        string UserName { get; }
    }
}