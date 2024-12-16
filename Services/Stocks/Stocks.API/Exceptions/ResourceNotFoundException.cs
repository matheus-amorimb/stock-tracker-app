namespace Stocks.API.Exceptions;

public class ResourceNotFoundException(string message) : Exception(message) { }