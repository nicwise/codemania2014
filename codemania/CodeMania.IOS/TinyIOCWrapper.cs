using System;
using BigTed.Core;
using TinyIoC;
using TinyMessenger;

namespace CodeMania.IOS
{
	public class TinyIoCWrapper : IContainer
	{
		public static void Init ()
		{
			BigTed.Core.Container.ContainerSource = new TinyIoCWrapper ();
			Container.Register<ITinyMessengerHub, TinyMessengerHub> ();
		}

		public static TinyIoCContainer Container
		{
			get
			{
				return TinyIoCContainer.Current;
			}
		}

		public static ITinyMessengerHub Hub
		{
			get
			{
				return Container.Resolve<ITinyMessengerHub> ();
			}
		}

		public void Register<T> () where T : class
		{
			Container.Register<T> ().AsMultiInstance();
		}

		public void RegisterSingleton<T>() where T : class 
		{
			Container.Register<T> ().AsSingleton ();
		}

		public T Resolve<T> () where T : class
		{
			return Container.Resolve<T> ();
		}

		public TinyMessageSubscriptionToken Subscribe<TMessage> (Action<TMessage> deliveryAction) where TMessage : class, ITinyMessage
		{
			return Hub.Subscribe<TMessage> (deliveryAction);
		}

		public void Unsubscribe<TMessage> (TinyMessageSubscriptionToken subscriptionToken) where TMessage : class, ITinyMessage
		{
			Hub.Unsubscribe<TMessage> (subscriptionToken);
		}

		public void Publish<TMessage> (TMessage message) where TMessage : class, ITinyMessage
		{
			Hub.Publish (message);
		}

		public void PublishAsync<TMessage> (TMessage message) where TMessage : class, ITinyMessage
		{
			Hub.PublishAsync (message);
		}
	}
}

