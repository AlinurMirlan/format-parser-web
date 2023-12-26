using System.Text;

namespace FormatParser.Library.Formats.CaseConvertors;

public abstract class PascalToLowerCase : ICaseConvertor
{
    protected abstract string Separator { get; set; }

    public string Convert(string identificator)
    {
        if (string.IsNullOrEmpty(identificator))
            return string.Empty;

        StringBuilder tokenBuilder = new();
        StringBuilder snakeBuilder = new();
        for (int i = 1; i <= identificator.Length; i++)
        {
            char character = identificator[^i];
            if (char.IsUpper(character))
            {
                tokenBuilder.Insert(0, char.ToLowerInvariant(character));
                string snakeToken = tokenBuilder.ToString();
                snakeBuilder.Insert(0, $"{Separator}{snakeToken}");
                tokenBuilder.Clear();
                continue;
            }

            tokenBuilder.Insert(0, character);
        }

        return snakeBuilder.ToString()[1..];
    }
}
