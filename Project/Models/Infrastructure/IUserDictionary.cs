namespace Models.Infrastructure
{
    public interface IUserDictionary
    {
        string Id { get; set; }
        string Name { get; set; }
        string SystemId { get; set; }
        bool Enable { get; set; }
        bool Selected { get; set; }
    }
}
