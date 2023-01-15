using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeVisualiser.Model;
using TypeVisualiser.Model.Persistence;

namespace TypeVisualiser.Demo.Infrastructure
{
    public class AssociationDataFactory : IAssociationDataFactory
    {
        public Type GetType(IAssociation association)
        {
            switch (association)
            {
                case ConsumeAssociation:
                    return typeof(ConsumeAssociationData);       
                case FieldAssociation:
                    return typeof(FieldAssociationData);
                default:
                    throw new Exception("%Y 5544 f");
            }
        }
    }
}
