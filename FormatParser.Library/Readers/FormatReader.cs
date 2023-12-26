using FormatParser.Library.Infrastructure;
using System.Runtime.Serialization;

namespace FormatParser.Library.Readers;

public abstract class FormatReader<TEntity> where TEntity : class
{
    public abstract TEntity Parse(DirectoryInfo formatDirectory);
    protected static T Parse<T>(DirectoryInfo formatDirectory, ParseFile<T> parseFile) where T : class
    {
        FileInfo[] files = formatDirectory.SearchFiles(parseFile.FileName);
        if (files.Length == 0)
            throw new FileNotFoundException($"{parseFile.FileName} not found.");

        FileInfo file = files[0];
        T? fileInfo = parseFile.FileParser.Parse(file.OpenRead())
            ?? throw new SerializationException($"{parseFile.FileName} could not be deserialized. Make sure it's in the correct format.");
        return fileInfo;
    }
}
