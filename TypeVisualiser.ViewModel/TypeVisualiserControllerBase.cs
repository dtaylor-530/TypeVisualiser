namespace TypeVisualiser.UI.WpfUtilities
{
    using System.Windows.Threading;
    using GalaSoft.MvvmLight;
    using Messaging;
    using StructureMap;
    using TypeVisualiser.Abstractions;

    public class TypeVisualiserControllerBase :  ViewModelBase
    {
        private readonly Dispatcher doNotUseDispatcher;
        private readonly IMessenger messenger;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // Required for testing

        // ReSharper restore FieldCanBeMadeReadOnly.Local
        private IUserPromptMessage doNotUseUserPrompt;

        public TypeVisualiserControllerBase(IContainer container)
        {
            // This relies on the Xaml being responsible for instantiating the controller.
            this.doNotUseDispatcher = Dispatcher.CurrentDispatcher;
            this.messenger = container.GetInstance<IMessenger>();
        }

        protected Dispatcher Dispatcher
        {
            get
            {
                return this.doNotUseDispatcher;
            }
        }

        protected IMessenger Messenger
        {
            get
            {
                return this.messenger;
            }
        }

        protected IUserPromptMessage UserPrompt
        {
            get
            {
                return this.doNotUseUserPrompt ?? (this.doNotUseUserPrompt = new WindowsMessageBox());
            }

            set
            {
                this.doNotUseUserPrompt = value;
            }
        }
    }
}