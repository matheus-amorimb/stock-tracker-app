namespace StocksNotification.Notifications.Email;

public class EmailSettings
{
    public required string SmtpServer { get; init; }
    public required int SmtpPort { get; init; }
    public required string SenderEmail { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public bool EnableSsl { get; init; } = true;
}

public class EmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;
    
    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
        _emailSettings = GetEmailSettings().Result;
    }
    
    public async Task SendEmailAsync(PriceAlertTriggeredEvent alertEvent)
    {
        var smtpClient = CreateSmtpClient();
        var mailMessage = await CreateMailMessage(alertEvent);
        
        try
        {
            await smtpClient.SendMailAsync(mailMessage);
            _logger.LogInformation($"[EMAIL] Email sent successfully to {alertEvent.SubscriberEmail}");
            
        }
        catch (Exception e)
        {
            _logger.LogInformation($"[EMAIL] Error while sending email to {alertEvent.SubscriberEmail}. \n " +
                                   $"Error: {e.Message}");
            smtpClient.Dispose();
            mailMessage.Dispose();
        }
    }

    private async Task<EmailSettings> GetEmailSettings()
    {
        var filePath = GetEmailSettingsFullPath("emailsettings.txt");        
        var rawContent = await File.ReadAllTextAsync(filePath);
        var emailSettings = JsonSerializer.Deserialize<EmailSettings>(rawContent);
        if (emailSettings == null)
        {
            throw new FileNotFoundException($"Email settings not found in file {filePath}");
        }
        foreach (var prop in typeof(EmailSettings).GetProperties())
        {
            var propValue = prop.GetValue(emailSettings);
            if (propValue != null) continue;
            throw new ArgumentNullException($"Email settings file is not configure correctly. The field {prop.Name} is missing.");
        }
        return emailSettings;
    }
    
    private string GetEmailSettingsFullPath(string fileName)
    {
        var solutionRoot = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
        var relativePath = $"Services/StocksNotification/StocksNotification/Notifications/Email/{fileName}";
        // var fullPath = Path.Combine(solutionRoot, relativePath);
        var fullPath = "/home/matheus/matheus-dev/code/projects/stock-tracker-app/Services/StocksNotification/StocksNotification/Notifications/Email/emailsettings.txt";
        return fullPath;
    }
    
    private SmtpClient CreateSmtpClient()
    {
        var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort);
        smtpClient.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password);
        smtpClient.EnableSsl = _emailSettings.EnableSsl;
        return smtpClient;
    }

    private async Task<MailMessage> CreateMailMessage(PriceAlertTriggeredEvent alertEvent)
    {
        var subject = $"STOCK ALERT FOR: {alertEvent.StockName}!";
        var body = await GetMessageBody(alertEvent);
        var recipient = alertEvent.SubscriberEmail;
        
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailSettings.SenderEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
            To = { recipient }
        };
        
        return mailMessage;
    }

    private async Task<string> GetMessageBody(PriceAlertTriggeredEvent alertEvent)
    {
        var emailPagePath = GetEmailPageFullPath("index.html");
        var rawContent = await File.ReadAllTextAsync(emailPagePath);
        var body = PopulatePage(rawContent, alertEvent);
        return body;
    }

    private string GetEmailPageFullPath(string pageName)
    {
        var solutionRoot = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
        var relativePath = $"Services/StocksNotification/StocksNotification/Notifications/Email/EmailPage/{pageName}";
        // var fullPath = Path.Combine(solutionRoot, relativePath);
        var fullPath = "/home/matheus/matheus-dev/code/projects/stock-tracker-app/Services/StocksNotification/StocksNotification/Notifications/Email/EmailPage/index.html";
        return fullPath;
    }

    private string PopulatePage(string rawContent, PriceAlertTriggeredEvent alertEvent)
    {
        var type = alertEvent.GetType();
        foreach (var prop in type.GetProperties())
        {
            rawContent = rawContent.Replace($"{{{{{prop.Name}}}}}", prop.GetValue(alertEvent)?.ToString());
        }
        return rawContent;
    }
}