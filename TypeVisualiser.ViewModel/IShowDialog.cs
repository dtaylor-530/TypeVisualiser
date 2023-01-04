using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TypeVisualiser.Model;
using TypeVisualiser.UI;

namespace TypeVisualiser.ViewModels
{
    public interface IShowDialog
    {
        string? ShowAnnotationInputBox(string text);
         bool? ShowChooseTypeDialog(ChooseTypeController controller);
         void ShowUsageDialog(IVisualisableTypeWithAssociations subject, FieldAssociation fieldAssociation);
    }
}
