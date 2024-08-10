namespace PlatformService.Services.RabbitMq_MassTransit
{
	public interface IDriverNotificationPublisherService
	{
		Task SendNotification(string PlatformName, string Publisher);
	}
}
