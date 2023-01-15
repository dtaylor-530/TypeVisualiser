using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using StructureMap;
using TypeVisualiser.Abstractions;
using TypeVisualiser.Geometry;
using TypeVisualiser.Library;
using TypeVisualiser.Model;
using TypeVisualiser.Models.Abstractions;

namespace TypeVisualiser.UI
{
    /// <summary>
    /// A class responsible for drawing the uml diagram given the collection of <see cref="IVisualisableType"/>s.
    /// </summary>
    internal class ClassUmlDrawingEngine
    {
        private readonly IContainer container;
        private readonly Abstractions.IMessenger messenger;
        private readonly IDiagramElementFactory diagramElementFactory;
        private readonly IConnectorBuilder connectorBuilder;
        private ICollection<IDiagramElement> allElements;

        /// <summary>
        /// This holds a temporary list of diagram elements that have been moved to their final position on the diagram.
        /// Used by the <see cref="PositionAllOtherAssociations"/> method.
        /// </summary>
        private List<IDiagramElement> positionedElements;

        private Func<IDiagramElement, bool, bool> shouldSecondaryElementBeVisible;



        public ClassUmlDrawingEngine(IContainer container,
            Guid diagramId,
            IVisualisableTypeWithAssociations mainSubject)
        {
            this.container = container;
            messenger = container.GetInstance<Abstractions.IMessenger>();
            diagramElementFactory = container.GetInstance<IDiagramElementFactory>();
            connectorBuilder = container.GetInstance<IConnectorBuilder>();
            DiagramId = diagramId;
            MainSubject = mainSubject;
            var subjectAssociation = container.GetInstance<SubjectAssociation>().Initialise(mainSubject);
            MainDrawingSubject = diagramElementFactory.Create(DiagramId, subjectAssociation, messenger);
        }

        public Guid DiagramId { get; private set; }

        public IDiagramElement MainDrawingSubject { get; private set; }

        public IVisualisableTypeWithAssociations MainSubject { get; private set; }

        public IEnumerable<IDiagramElement> DrawAllBoxes()
        {
            var addedElements = new List<IDiagramElement> { MainDrawingSubject };

            AddDiagramElementForParentAssociation(MainSubject.Parent, addedElements);
            foreach (ParentAssociation @interface in MainSubject.ThisTypeImplements)
            {
                AddDiagramElementForParentAssociation(@interface, addedElements);
            }

            addedElements.AddRange(MainSubject.AllAssociations.Select(association => diagramElementFactory.Create(DiagramId, association, messenger)));

            return addedElements;
        }

        /// <summary>
        /// Draw the lines to connect all types in the diagram.
        /// </summary>
        /// <param name="allDiagramElements">All the diagram types already on diagram surface and arranged.</param>
        /// <param name="shouldSecondaryElementBeVisibleDelegate">A delegate that determines if secondary associations are configured to be visible.</param>
        /// <param name="secondaryElements">An out param that returns the secondary associations so they can be shown/hidden independently of the main elements.</param>
        /// <returns>A collection of added elements that wrapping the lines that have been created.</returns>
        public IEnumerable<IDiagramElement> DrawConnectingLines(ICollection<IDiagramElement> allDiagramElements,
                                                               Func<IDiagramElement, bool, bool> shouldSecondaryElementBeVisibleDelegate,
                                                               out Dictionary<string, IDiagramElement> secondaryElements)
        {
            if (allDiagramElements.Any(x => x.DiagramContent is IConnectionLine))
            {
                throw new InvalidOperationException("Code error: Draw Association Lines cannot be called twice.");
            }

            var addedElements = new List<IDiagramElement>();
            secondaryElements = new Dictionary<string, IDiagramElement>();
            this.allElements = allDiagramElements;
            this.shouldSecondaryElementBeVisible = shouldSecondaryElementBeVisibleDelegate;
            foreach (IDiagramElement associateElement in allDiagramElements.ToList()) // Loop through a copy of the collection. The loop will add new content to it.
            {
                if (associateElement.DiagramContent is SubjectAssociation)
                {
                    continue;
                }

                DrawLine(addedElements, associateElement, secondaryElements);
            }

            this.allElements = null;
            this.shouldSecondaryElementBeVisible = null;
            return addedElements;
        }

        public void PositionAllOtherAssociations(ICollection<IDiagramElement> allDiagramElements)
        {
            this.positionedElements = new List<IDiagramElement>();
            Area subjectArea = MainDrawingSubject.Area;
            this.positionedElements.Add(MainDrawingSubject);

            foreach (IDiagramElement diagramElement in allDiagramElements)
            {
                var calculatablePositionContent = diagramElement.DiagramContent as ICalculatedPositionDiagramContent;
                if (calculatablePositionContent != null)
                {
                   
                    // Only certain diagram elements should have their position calculated. The rest follow suit with those that have had their position calculated.
                    Area area = calculatablePositionContent.ProposePosition(diagramElement.Width, diagramElement.Height, subjectArea, IsOverlappingWithOtherControls);
                    diagramElement.TopLeft = area.TopLeft;
                    this.positionedElements.Add(diagramElement); // Used for IsOverlappingWithOtherControls (no need to care about overlapping with elements not yet positioned.
                }
            }

            this.positionedElements.Clear();
        }

        public void PositionMainSubject(IDiagram hostDiagram)
        {
            MainDrawingSubject.CenterOnPoint(hostDiagram.Centre);
        }

        private void AddDiagramElementForParentAssociation(ParentAssociation parent, IList<IDiagramElement> addedElements)
        {
            if (parent == null)
            {
                return;
            }

            addedElements.Add(diagramElementFactory.Create(DiagramId, parent, messenger));

            var parentType = parent.AssociatedTo as IVisualisableTypeWithAssociations;
            if (parentType != null)
            {
                if (parentType.ThisTypeImplements != null)
                {
                    foreach (ParentAssociation grandParent in parentType.ThisTypeImplements)
                    {
                        AddDiagramElementForParentAssociation(grandParent, addedElements);
                    }
                }

                if (parentType.Parent != null)
                {
                    AddDiagramElementForParentAssociation(parentType.Parent, addedElements);
                }
            }
        }

        private void AppendToCollections(List<IDiagramElement> addedElements, Dictionary<string, IDiagramElement> secondaryElements, IEnumerable<IDiagramElement> results)
        {
            foreach (IDiagramElement element in results)
            {
                addedElements.Add(element);
                secondaryElements.Add(element.DiagramContent.Id, element);
                element.IsVisibleAdditionalLogic = this.shouldSecondaryElementBeVisible;
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IDiagramContent", Justification = "Known context word")]
        private void DrawLine(List<IDiagramElement> addedElements, IDiagramElement associateElement, Dictionary<string, IDiagramElement> secondaryElements)
        {
            var association = associateElement.DiagramContent as Association;
            if (association == null)
            {
                throw new InvalidCastException("Code Error: Incorrect IDiagramContent type used with type dependency diagram, it must be an Association derivative.");
            }

            // Draw the primary line from the diagram's subject to the association. This could also be a line from the subject's parent to a grandparent.
            IDiagramElement fromElement = GetSourceOfLine(association); // Sometimes returns the subject or a parent connecting to a grandparent.
            addedElements.AddRange(DrawLine(fromElement, associateElement, association));

            // Draw any lines from this diagram element to its associations that are included on the diagram.
            IEnumerable<IDiagramElement> results = DrawLineForSecondaryParent(associateElement, association);
            AppendToCollections(addedElements, secondaryElements, results);

            results = DrawLineForSecondaryImplements(associateElement, association);
            AppendToCollections(addedElements, secondaryElements, results);

            results = DrawLineForSecondaryAssociations(associateElement, association);
            AppendToCollections(addedElements, secondaryElements, results);
        }

        private IEnumerable<IDiagramElement> DrawLine(IDiagramElement fromElement, IDiagramElement destinationElement, Association destinationAssociation)
        {
            Logger.Instance.WriteEntry("DrawLine for   {0}", destinationAssociation.AssociatedTo.Name);
            Logger.Instance.WriteEntry("    From Area: {0}", fromElement.Area);
            Logger.Instance.WriteEntry("    To Area  : {0}", destinationElement.Area);

            IConnectionLine route = connectorBuilder.CalculateBestConnection(fromElement.Area, destinationElement.Area, IsOverlappingWithOtherControls);
            destinationAssociation.StyleLine(route);
            var addedElements = new List<IDiagramElement>();
            //Logger.Instance.WriteEntry("    Route calculated from {0:F1}, {1:F1}  to {2:F1}, {3:F1}", route.From.X, route.From.Y, route.To.X, route.To.Y);
            //Logger.Instance.WriteEntry("    From angle {0:F1} to angle {1:F1}", route.FromAngle, route.ToAngle);

            // Create the line diagram element and add to the diagram collection.
            // The line is linked to the arrow head position.
            var x = new DiagramElementConstruction() { TopLeft = route.From };
            var lineDiagramElement = diagramElementFactory.Create(DiagramId, route, messenger, x);
            lineDiagramElement.RegisterPositionDependency(new[] { fromElement, destinationElement }, IsOverlappingWithOtherControls);
            addedElements.Add(lineDiagramElement);

            // Create an arrow head based on the best route and add to the diagram collection.
            // The arrow head is linked to the associate diagram element.
            var arrowHead = destinationAssociation.CreateLineHead();
            var y = new DiagramElementConstruction() { TopLeft = route.To };
            var headDiagramElement = diagramElementFactory.Create(DiagramId, arrowHead, messenger, y);
            headDiagramElement.RegisterPositionDependency(new[] { lineDiagramElement }, IsOverlappingWithOtherControls);
            addedElements.Add(headDiagramElement);

            return new IDiagramElement[] { lineDiagramElement, headDiagramElement };
        }

        private IEnumerable<IDiagramElement> DrawLineForSecondaryAssociations(IDiagramElement fromDiagramElement, Association association)
        {
            var fullyExpandedModelType = association.AssociatedTo as IVisualisableTypeWithAssociations;
            if (fullyExpandedModelType == null || !fullyExpandedModelType.AllAssociations.Any())
            {
                return new List<IDiagramElement>();
            }

            var addedElements = new List<IDiagramElement>();
            foreach (FieldAssociation fieldAssociation in fullyExpandedModelType.AllAssociations)
            {
                IDiagramElement destinationElement = FindDiagramElementFromContentId(fieldAssociation.Id);
                if (destinationElement == null)
                {
                    continue;
                }

                var destinationAssociation = destinationElement.DiagramContent as Association;
                addedElements.AddRange(DrawLine(fromDiagramElement, destinationElement, destinationAssociation));
            }

            return addedElements;
        }

        private IEnumerable<IDiagramElement> DrawLineForSecondaryImplements(IDiagramElement fromDiagramElement, Association association)
        {
            var fullyExpandedModelType = association.AssociatedTo as IVisualisableTypeWithAssociations;
            if (fullyExpandedModelType == null || !fullyExpandedModelType.ThisTypeImplements.Any())
            {
                return new List<IDiagramElement>();
            }

            var addedElements = new List<IDiagramElement>();
            foreach (ParentAssociation implement in fullyExpandedModelType.ThisTypeImplements)
            {
                IDiagramElement destinationElement = FindDiagramElementFromContentId(implement.Id);
                if (destinationElement == null)
                {
                    continue;
                }

                var destinationAssociation = destinationElement.DiagramContent as Association;
                addedElements.AddRange(DrawLine(fromDiagramElement, destinationElement, destinationAssociation));
            }

            return addedElements;
        }

        private IEnumerable<IDiagramElement> DrawLineForSecondaryParent(IDiagramElement fromDiagramElement, Association parentAssociation)
        {
            var fullyExpandedModelType = parentAssociation.AssociatedTo as IVisualisableTypeWithAssociations;
            if (fullyExpandedModelType == null || fullyExpandedModelType.Parent == null)
            {
                return new List<IDiagramElement>();
            }

            IDiagramElement destinationElement = FindDiagramElementFromContentId(fullyExpandedModelType.Parent.Id);
            if (destinationElement == null)
            {
                return new List<IDiagramElement>();
            }

            var destinationAssociation = destinationElement.DiagramContent as Association;
            return DrawLine(fromDiagramElement, destinationElement, destinationAssociation);
        }

        private IDiagramElement FindDiagramElementFromContentId(string id)
        {
            return this.allElements.FirstOrDefault(e => e.DiagramContent.Id == id && e.DiagramContent is Association);
        }

        private IDiagramElement GetSourceOfLine(Association association)
        {
            var parentAssociation = association as ParentAssociation;
            if (parentAssociation == null)
            {
                return MainDrawingSubject;
            }

            IDiagramElement search = this.allElements.FirstOrDefault(e => e.DiagramContent.Id == parentAssociation.AssociatedFrom.Id);
            if (search == null)
            {
                return MainDrawingSubject;
            }

            return search;
        }

        /// <summary>
        /// Determines whether the given area is overlapping with other areas occupied by controls.
        /// </summary>
        /// <param name="proposedArea">The proposed area to compare with all others.</param>
        /// <returns>A result object indicating if an overlap exists or the closest object and distance to it.</returns>
        private ProximityTestResult IsOverlappingWithOtherControls(Area proposedArea)
        {
            // Only check for overlapps with elements that are already in their final position.
            List<ProximityTestResult> proximities = this.positionedElements.Select(diagramElement => diagramElement.Area.OverlapsWith(proposedArea)).ToList();
            bool overlapsWith = proximities.Any(result => result.Proximity == Proximity.Overlapping);
            if (overlapsWith)
            {
                return new ProximityTestResult(Proximity.Overlapping);
            }

            IOrderedEnumerable<ProximityTestResult> veryClose = proximities.Where(x => x.Proximity == Proximity.VeryClose).OrderBy(x => x.DistanceToClosestObject);

            if (veryClose.Any())
            {
                return veryClose.First();
            }

            return new ProximityTestResult(Proximity.NotOverlapping);
        }
    }
}