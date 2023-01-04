﻿namespace TypeVisualiser
{
    using System;
    using System.Windows;
    using System.Windows.Threading;

    using GalaSoft.MvvmLight.Messaging;
    using StructureMap;
    using TypeVisualiser.Demo;
    using TypeVisualiser.ILAnalyser;
    using TypeVisualiser.Messaging;
    using TypeVisualiser.Model;
    using TypeVisualiser.RecentFiles;
    using TypeVisualiser.Startup;
    using TypeVisualiser.UI;
    using TypeVisualiser.ViewModels;

    /// <summary>
    /// Interaction logic for the main Application class.
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.DispatcherUnhandledException += this.OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += this.OnCurrentDomainUnhandledException;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += ModelBuilder.OnAssemblyResolve;

            Current.Exit += this.OnApplicationExit;
            IoC.MapHardcodedRegistrations(Intialise());

            new UI.Views.Shell().Show();
        }

        private static void LogUnhandledException(string origin, object ex)
        {
            Logger.Instance.WriteEntry(string.Empty);
            Logger.Instance.WriteEntry("=====================================================================================");
            Logger.Instance.WriteEntry(DateTime.Now);
            Logger.Instance.WriteEntry("Unhandled exception was thrown from orgin: " + origin);
            string message = ex.ToString();
            Logger.Instance.WriteEntry(message);

            new WindowsMessageBox().Show("TypeVisualiser.Properties.Resources.Application_An_Unhandled_Exception_Occurred", (object)message);
            //new WindowsMessageBox().Show(TypeVisualiser.Properties.Resources.Application_An_Unhandled_Exception_Occurred, (object)message);
            Current.Shutdown();
        }

        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            Current.Exit -= this.OnApplicationExit;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= ModelBuilder.OnAssemblyResolve;
            Messenger.Default.Send(new ShutdownMessage());
        }

        private void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogUnhandledException("App.OnCurrentDomainUnhandledException", e.ExceptionObject);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogUnhandledException("App.OnDispatcherUnhandledException", e.Exception);
        }

        private static Container Intialise()
        {
            return new Container(
                config =>
                    {
                        config.For<IApplicationResources>().Use<DefaultApplicationResources>();
                        config.For<IConnectorBuilder>().Use<FixedPointCollectionConnectorBuilder>().Named(ConnectorType.Snap.ToString());
                        config.For<IConnectorBuilder>().Use<DirectLineConnectorBuilder>().Named(ConnectorType.Direct.ToString());
                        config.For<IDiagramDimensions>().Singleton().Use<DiagramDimensions>();

                        // TODO this is broken - shouldnt be a singleton there is one per diagram. Its only working because it is refreshed each time a diagram is displayed.
                        config.For<IFileManager>().Use<FileManager>();
                        config.For<IMessenger>().Use(Messenger.Default);
                        config.For<IMethodBodyReader>().Use<MethodBodyReader>();
                        config.For<IModelBuilder>().Use<ModelBuilder>();
                        config.For<IRecentFiles>().Use<RecentFilesXml>();
                        config.For<ITrivialFilter>().Singleton().Use<TrivialFilter>();
                        config.For<IVisualisableType>().Use<VisualisableType>();
                        config.For<IVisualisableTypeWithAssociations>().Use<VisualisableTypeWithAssociations>();
                        config.For<IShowDialog>().Use<ShowDialog>();
                    });
        }
    }
}