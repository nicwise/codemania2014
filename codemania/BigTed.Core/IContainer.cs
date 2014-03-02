using System;
using TinyMessenger;

namespace BigTed.Core
{
	public interface IContainer
	{
		void Register<T> () where T : class;
		void RegisterSingleton<T>() where T : class;
		T Resolve<T>() where T : class;
		TinyMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction) where TMessage : class, ITinyMessage;
		void Unsubscribe<TMessage>(TinyMessageSubscriptionToken subscriptionToken) where TMessage : class, ITinyMessage;
		void Publish<TMessage>(TMessage message) where TMessage : class, ITinyMessage;
		void PublishAsync<TMessage>(TMessage message) where TMessage : class, ITinyMessage;

	}
}

