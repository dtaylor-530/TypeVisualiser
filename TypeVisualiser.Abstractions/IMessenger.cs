using GalaSoft.MvvmLight.Messaging;

namespace TypeVisualiser.Abstractions
{
    public interface IMessenger
    {
        void Register<T>(object recipient, Action<T> onRemoveRecentlyUsedFileRequested) where T : MessageBase;
        void Unregister<T>(object recipient) where T : MessageBase;
    }
}
