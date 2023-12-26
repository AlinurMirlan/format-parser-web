using FormatParser.Library.Formats.EntityBuilders;
using FormatParser.Library.Formats.CaseConvertors;

namespace FormatParser.Library.Formats.Parsers;

public abstract class FormatParser<TEntity> where TEntity : class
{
    public TEntity? Parse(string filePath) => Convert(File.OpenRead(filePath));

    public TEntity? Parse(Stream fileStream) => Convert(fileStream);

    protected virtual IEntityBuilder EntityBuilder { get; set; }

    protected abstract ICaseConvertor? CaseConvertor { get; set; }

    protected FormatParser()
    {
        EntityBuilder = new EntityBuilder(CaseConvertor);
    }

    protected abstract Entity ExtractEntity(Stream fileStream);

    protected TEntity? Convert(Stream stream)
    {
        Entity nameValuePairs = ExtractEntity(stream);
        return EntityBuilder.BuildEntity(typeof(TEntity), nameValuePairs) as TEntity;
    }
}
