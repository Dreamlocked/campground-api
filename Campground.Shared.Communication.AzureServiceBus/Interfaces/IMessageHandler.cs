namespace Campground.Shared.Communication.AzureServiceBus.Interfaces
{
    public interface IMessageHandler
    {
        Task HandleMessageAsync(string message);
    }
}
