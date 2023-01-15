using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeVisualiser.Abstractions;
using TypeVisualiser.Library;
using TypeVisualiser.Model;
using TypeVisualiser.WPF.Common;

namespace TypeVisualiser.Demo.Infrastructure
{
    public class LineHeadFactory : ILineHeadFactory
    {
        public IDiagramContentFunctionality CreateLineHead(IAssociation association)
        {
            switch (association)
            {
                //case ConsumeAssociation field:
                //    return new AssociationArrowHead();
                case FieldAssociation:
                case SubjectAssociation:
                    return new AssociationArrowHead();
                case ParentAssociation:
                    return new InheritanceArrowHead();
                default:
                    throw new Exception("vfdr4 de");
            }
        }
    }
}
