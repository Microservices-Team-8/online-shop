namespace OnlineShop.Email.Console;

public class RabbitMQOptions
{
    public const string SectionName = "RabbitMQ";
    
    public string Host { get; set; }
    public string EmailExchange { get; set; }
    public string EmailSendQueue { get; set; }
}