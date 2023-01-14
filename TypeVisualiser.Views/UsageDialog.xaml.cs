namespace TypeVisualiser.UI.Views
{
    using System.Windows;
    using Model;
    using StructureMap;

    /// <summary>
    /// Code behind for UsageDialog
    /// </summary>
    public partial class UsageDialog
    {
        private readonly IContainer container;

        /// <summary>
        /// Initializes a new instance of the UsageDialog class.
        /// </summary>
        public UsageDialog(IContainer container)
        {
            InitializeComponent();
            this.container = container;
        }

        public void ShowDialog(string title, string subjectName, string subjectType, FieldAssociation association)
        {
            DataContext = new UsageDialogController(container, title, subjectName, subjectType, association);
            ShowDialog();
        }

        private void OnDefaultButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}