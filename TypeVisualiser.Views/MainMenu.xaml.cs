using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using TypeVisualiser.Messaging;

namespace TypeVisualiser.UI.Views
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu
    {
        public MainMenu()
        {
            InitializeComponent();
            //MessagingGate.Register<RecentFileDeleteMessage>(this, OnDeleteRecentlyUsedFile);
            this.Loaded += MainMenu_Loaded;
        }

        private void MainMenu_Loaded(object sender, RoutedEventArgs e)
        {
            ShellController.DeleteFile += OnRecentlyUsedFilesLoaded;
            this.RecentlyUsedFiles.Loaded += (s, e) => OnRecentlyUsedFilesLoaded();
        }

        private void ShellController_DeleteFile()
        {
            throw new System.NotImplementedException();
        }

        public ShellController ShellController => this.DataContext as ShellController;

        private void OnDeleteRecentlyUsedFile(RecentFileDeleteMessage message)
        {
            bool found = false;
            MenuItem menuItem = null;
            foreach (object item in this.RecentlyUsedFiles.Items)
            {
                menuItem = item as MenuItem;
                if (menuItem != null && menuItem.CommandParameter == message.Data)
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                this.RecentlyUsedFiles.Items.Remove(menuItem);
            }
        }

        private void OnRecentlyUsedFilesLoaded()
        {
            this.RecentlyUsedFiles.Items.SortDescriptions.Add(new SortDescription("When", ListSortDirection.Descending));
        }
    }
}