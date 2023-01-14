namespace TypeVisualiser.Messaging
{
    using System;
    using mbox = GalaSoft.MvvmLight.Messaging;
    using TypeVisualiser.Abstractions;
    using TypeVisualiser.Library;

    public class MessagingGate : IMessenger
    {
        private static readonly object SyncRoot = new object();

        public void Register<T>(object recipient, Action<T> handler) where T : mbox.MessageBase
        {
            lock (SyncRoot)
            {
                mbox.Messenger.Default.Register(recipient, handler);
            }
        }

        public void Unregister<T>(object recipient) where T : mbox.MessageBase
        {
            lock (SyncRoot)
            {
                mbox.Messenger.Default.Unregister<T>(recipient);
            }
        }

        /// <summary>
        /// Sends the specified message.
        /// Use this when you are concerned about concurrency. Sending messages at the same time as registering new listeners on many threads will cause issues.
        /// </summary>
        /// <typeparam name="T">The message to send</typeparam>
        /// <param name="message">The message.</param>
        public void Send<T>(T message) where T : mbox.MessageBase
        {
            lock (SyncRoot)
            {
                mbox.Messenger.Default.Send(message);
            }
        }
    }
}
