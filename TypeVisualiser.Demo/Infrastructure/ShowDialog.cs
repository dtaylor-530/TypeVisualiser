using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
using TypeVisualiser.Model;
using TypeVisualiser.UI;
using TypeVisualiser.UI.Views;
using TypeVisualiser.ViewModels;

namespace TypeVisualiser.Demo
{
    public class ShowDialog : IShowDialog
    {
        private readonly IContainer container;

        public ShowDialog(IContainer container)
        {
            this.container = container;
        }

        public string? ShowAnnotationInputBox(string text)
        {
            var inputBox = new AnnotationInputBox { InputText = text };
            inputBox.ShowDialog();
            return inputBox.InputText;
        }

        public bool? ShowChooseTypeDialog(ChooseTypeController controller)
        {
            var chooseType = new ChooseType { DataContext = controller, ResizeMode = ResizeMode.NoResize };
            return chooseType.ShowDialog();
        }

        public void ShowUsageDialog(IVisualisableTypeWithAssociations subject, FieldAssociation fieldAssociation)
        {
             new UsageDialog(container).ShowDialog("Resources.ApplicationName", subject.Name, subject.Modifiers.TypeTextName, fieldAssociation);
        }
    }
}

