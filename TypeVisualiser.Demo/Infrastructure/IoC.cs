namespace TypeVisualiser.Startup
{
    using System.Diagnostics.CodeAnalysis;

    using GalaSoft.MvvmLight.Messaging;

    using StructureMap;

    using TypeVisualiser.ILAnalyser;
    using TypeVisualiser.Model;
    using TypeVisualiser.RecentFiles;

    public static class IoC
    {
        private static readonly object SyncRoot = new object();

        private static volatile IContainer container;

        public static IContainer Default
        {
            get
            {
                if (container == null)
                {
                    lock (SyncRoot)
                    {
                        if (container == null)
                        {
                            container = new Container();
                        }
                    }
                }

                return container;
            }
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "IoC Registrations here prevent excessive coupling elsewhere.")]
        public static void MapHardcodedRegistrations(Container _container)
        {
            container = _container;
 
        }
    }
}