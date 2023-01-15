using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using TypeVisualiser.Library;
using TypeVisualiser.UI;
using TypeVisualiser.Models.Abstractions;
using System.Windows.Controls;

namespace TypeVisualiser.WPF.Templates
{
    public partial class Templates
    {
        private void OnLineClicked(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            var lineOrArrowhead = sender as DependencyObject;
            IDiagramElement element;
            do
            {
                if (lineOrArrowhead == null)
                {
                    throw new InvalidCastException("Unable to cast sender to Dependency Object");
                }

                var bindable = lineOrArrowhead as FrameworkElement;
                if (bindable != null && bindable.DataContext is IDiagramElement)
                {
                    element = bindable.DataContext as IDiagramElement;
                    break;
                }

                lineOrArrowhead = VisualTreeHelper.GetParent(lineOrArrowhead);
            } while (true);


            IDiagram diagram;

            do
            {

                var bindable = lineOrArrowhead as FrameworkElement;
                if (bindable != null && bindable.DataContext is IDiagram)
                {
                    diagram = bindable.DataContext as IDiagram;
                    break;
                }

                lineOrArrowhead = VisualTreeHelper.GetParent(lineOrArrowhead);
            } while (true);



            diagram.Controller.ShowLineDetails(element);
        }
    }
}
