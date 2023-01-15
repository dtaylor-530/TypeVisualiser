namespace TypeVisualiser.Model
{
    using System;
    using TypeVisualiser.Abstractions;
    using TypeVisualiser.Geometry;
    using TypeVisualiser.WPF.Common;

    public class SubjectAssociation : Association
    {
        private readonly ILineHeadFactory lineHeadFactory;

        public SubjectAssociation(
            ILineHeadFactory lineHeadFactory,
            IApplicationResources resources,
            ITrivialFilter trivialFilter)
            : base(resources, trivialFilter)
        {
            this.lineHeadFactory = lineHeadFactory;
        }

        public override string Name
        {
            get
            {
                return "Subject";
            }
        }

        public SubjectAssociation Initialise(IVisualisableTypeWithAssociations subject)
        {
            this.AssociatedTo = subject;
            this.IsInitialised = true;
            return this;
        }

        public override Area ProposePosition(double actualWidth, double actualHeight, Area subjectArea, Func<Area, ProximityTestResult> overlapsWithOthers)
        {
            return subjectArea;
        }

        public override IDiagramContentFunctionality CreateLineHead() => lineHeadFactory.CreateLineHead(this);
        //{
        //    // A subject association may still be asked to style an arrowhead if a secondary relationship points back to the subject.
        //    return new AssociationArrowHead();
        //}

        public override void StyleLine(IConnectionLine line)
        {
            // A subject association may still be asked to style a line if a secondary relationship points back to the subject.
            FieldAssociation.StyleLineForNonParentAssociation(line, 1, this.AssociatedTo, this.IsTrivialAssociation());
        }
    }
}