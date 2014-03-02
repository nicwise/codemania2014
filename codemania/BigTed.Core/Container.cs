using System;
using TinyMessenger;

namespace BigTed.Core
{
	public class Container
	{
		public static IContainer ContainerSource { get; set; }

		public static void Register<T> () where T : class
		{
			ContainerSource.Register<T> ();
		}

		public static void RegisterSingleton<T>() where T : class
		{
			ContainerSource.RegisterSingleton<T> ();
		}

		public static T Resolve<T> () where T : class
		{
			return ContainerSource.Resolve<T> ();
		}

		public static TinyMessageSubscriptionToken Subscribe<TMessage> (Action<TMessage> deliveryAction) where TMessage : class, ITinyMessage
		{
			return ContainerSource.Subscribe<TMessage> (deliveryAction);
		}

		public static void Unsubscribe<TMessage> (TinyMessageSubscriptionToken subscriptionToken) where TMessage : class, ITinyMessage
		{
			ContainerSource.Unsubscribe<TMessage> (subscriptionToken);
		}

		public static void Publish<TMessage> (TMessage message) where TMessage : class, ITinyMessage
		{
			ContainerSource.Publish<TMessage> (message);
		}

		public static void PublishAsync<TMessage> (TMessage message) where TMessage : class, ITinyMessage
		{
			ContainerSource.PublishAsync<TMessage> (message);
		}
	}
}

