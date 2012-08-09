using System.IO;
using Scaffold.Configuration;
using Scaffold.Exceptions;
using Scaffold.Io;

namespace Scaffold.Entities
{
    public class EntityManager : IEntityManager
    {
        private readonly IEntityReader entityReader;
        private readonly IConfiguration configuration;
        private readonly IFileSystem fileSystem;

        public EntityManager(IEntityReader entityReader, IConfiguration configuration, IFileSystem fileSystem)
        {
            this.entityReader = entityReader;
            this.configuration = configuration;
            this.fileSystem = fileSystem;
        }

        public Entity ReadEntity(string name)
        {
            var entityFile = configuration.CoreNameSpace + "\\Domain\\Model\\" + name + ".cs";
            if (!fileSystem.FileExists(entityFile))
            {
                throw new EntityNotFoundException("Entity not found: " + name + " at " + entityFile);
            }

            return entityReader.ReadEntity(entityFile);
        }

    }
}
