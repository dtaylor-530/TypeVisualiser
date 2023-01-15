namespace TypeVisualiser.Model
{
    public interface IAssociationDataFactory
    {
        public Type GetType(IAssociation association);
    }
}