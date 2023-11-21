namespace LibraryServices.EventBus.Abstraction
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
