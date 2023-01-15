using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeVisualiser.Library;
using TypeVisualiser.Model;
using TypeVisualiser.WPF.Common;

namespace TypeVisualiser.Abstractions
{
    public interface ILineHeadFactory
    {
        IDiagramContentFunctionality CreateLineHead(IAssociation association);

    }
}
