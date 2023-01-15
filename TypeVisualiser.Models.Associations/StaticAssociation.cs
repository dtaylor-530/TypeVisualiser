namespace TypeVisualiser.Model
{
    using System;
    using System.Collections.Generic;
    using TypeVisualiser.Abstractions;
    using TypeVisualiser.Model.Persistence;
    using TypeVisualiser.Models.Abstractions;

    public class StaticAssociation : ConsumeAssociation
    {
        IAssociationDataFactory associationDataFactory;

        public StaticAssociation(
            IAssociationDataFactory associationDataFactory,
            ILineHeadFactory lineHeadFactory,
            IApplicationResources resources,
            ITrivialFilter trivialFilter, 
            IModelBuilder modelBuilder,
            IDiagramDimensions diagramDimensions)
            : base(associationDataFactory, lineHeadFactory, resources, trivialFilter, modelBuilder, diagramDimensions)
        {
            this.associationDataFactory = associationDataFactory;
        }

        internal override Type PersistenceType=> associationDataFactory.GetType(this);

        public new StaticAssociation Initialise(Type associatedTo, int numberOfUsages, IEnumerable<AssociationUsageDescriptor> usageDescriptors, int depth)
        {
            return base.Initialise(associatedTo, numberOfUsages, usageDescriptors, depth) as StaticAssociation;
        }
    }
}