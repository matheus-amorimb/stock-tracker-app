namespace Stocks.API.Stock.ValueObjects;

public class Email
{
    public string Value { get;}
    
    private Email(string emailAddress)
    {
        Value = emailAddress;
    }
    
    public static Email Of(string emailAddress)
    { 
        if (IsValidEmail(emailAddress)) return new Email(emailAddress);
        throw new FormatException($"'{emailAddress}' is not a valid email address.");
    }
    
    private static bool IsValidEmail(string emailAddress)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
            return false;

        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(emailAddress);
    }
}