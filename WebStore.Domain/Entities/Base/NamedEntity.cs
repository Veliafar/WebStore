using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities.Base
{
    public abstract class NamedEntity : IEntity, INamedEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
