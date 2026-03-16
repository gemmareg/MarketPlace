namespace MarketPlace.Host.Abstractions.Security
{
    public interface ICurrentUser
    {
        string? UserId { get; }
        string? Email { get; }
        string? Name { get; }
        IEnumerable<string> Roles { get; }
    }
}
