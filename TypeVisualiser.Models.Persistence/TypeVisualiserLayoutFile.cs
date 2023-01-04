using System;
using System.Xml.Serialization;

namespace TypeVisualiser.Model.Persistence
{
    [Serializable]
    [XmlRoot(ElementName = "VisualisableTypeDiagramFile", Namespace = "http://typevisualiser.rees.biz/v1_5")]
    public class TypeVisualiserLayoutFile : ITypeVisualiserLayoutFile, IPersistentFileData
    {
        [XmlAttribute]
        public string AssemblyFileName { get; set; }

        [XmlAttribute]
        public string AssemblyFullName { get; set; }

        public CanvasLayoutData CanvasLayout { get; set; }

        [XmlAttribute]
        public string FileVersion { get; set; }

        [XmlAttribute]
        // TODO no longer required - remove next change.
            public bool HideParents { get; set; }

        [XmlAttribute]
        public bool HideTrivialTypes { get; set; }

        public IVisualisableTypeData Subject { get; set; }
    }
}