using System;
using TinyMessenger;

namespace BigTed.Core
{
	public interface IContainer
	{
		void Register<T>() where T : class;

		void Register<TIntf, TImpl>()  where TIntf : class where TImpl : class, TIntf;

		void RegisterSingleton<T>() where T : class;

		void RegisterSingleton<TIntf, TImpl>()  where TIntf : class where TImpl : class, TIntf;

		T Resolve<T>() where T : class;

		TinyMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction) where TMessage : class, ITinyMessage;

		void Unsubscribe<TMessage>(TinyMessageSubscriptionToken subscriptionToken) where TMessage : class, ITinyMessage;

		void Publish<TMessage>(TMessage message) where TMessage : class, ITinyMessage;

		void PublishAsync<TMessage>(TMessage message) where TMessage : class, ITinyMessage;
	}
}

