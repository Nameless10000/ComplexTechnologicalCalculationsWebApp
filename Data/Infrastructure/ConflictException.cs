namespace Data.Infrastructure;

public sealed class ConflictException(string message) : Exception(message);
