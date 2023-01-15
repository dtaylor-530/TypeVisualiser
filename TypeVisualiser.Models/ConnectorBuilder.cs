using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeVisualiser.Geometry;
using TypeVisualiser.Model;
using TypeVisualiser.Models.Abstractions;
using TypeVisualiser.UI;

namespace TypeVisualiser.ViewModels
{


    public class ConnectorBuilder : IConnectorBuilder, IConnectorBuilderSetter
    {
        private readonly IContainer container;
        private IConnectorBuilder connectorBuilder;

        public ConnectorBuilder(IContainer container)
        {
            this.container = container;
        }

        public IConnectionLine CalculateBestConnection(Area fromArea, Area destinationArea, Func<Area, ProximityTestResult> isOverlappingWithOtherControls)
        {
            return connectorBuilder.CalculateBestConnection(fromArea, destinationArea, isOverlappingWithOtherControls);
        }


        public void Set(ConnectorType connectorType)
        {
            connectorBuilder = container.GetInstance<IConnectorBuilder>(connectorType.ToString());
        }
    }
}
