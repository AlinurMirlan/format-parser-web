namespace FormatParser.Library.Formats;

public class InvalidFormatException : Exception
{
    public InvalidFormatException()
    {
    }

    public InvalidFormatException(string message) : base($"Incorrect format. {message}")
    {
    }

    public InvalidFormatException(string message, Exception inner) : base(message, inner)
    {
    }
}
