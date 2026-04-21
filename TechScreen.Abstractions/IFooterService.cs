namespace TechScreen.Abstractions
{
    public interface IFooterService
    {
        Task<string> GetFooterText(DateOnly today);
    }
}