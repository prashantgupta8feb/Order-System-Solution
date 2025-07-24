namespace NotificationService.Config
{
    public class RabbitMQSettings
    {
        public string Uri { get; set; } = string.Empty;
        public string QueueName { get; set; } = "notification_queue";
    }
}
