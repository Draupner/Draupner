namespace Scaffold.Entities
{
    public interface IEntityReader
    {
        Entity ReadEntity(string sourcePath);
    }
}