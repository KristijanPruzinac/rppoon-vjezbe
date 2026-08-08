using System.Collections.Generic;

namespace Rppoon.Zadaci.Promatrac.C1
{
    // ============ REFERENTNO RJESENJE - Promatrac C1 ============

    public interface ISubscriber
    {
        void OnNewVideo(string channel, string url);
    }

    public class MooTuber
    {
        private readonly List<ISubscriber> subscribers = new List<ISubscriber>();

        public MooTuber(string channel)
        {
            this.Channel = channel;
        }

        public string Channel { get; private set; }

        public void Subscribe(ISubscriber subscriber)
        {
            this.subscribers.Add(subscriber);
        }

        public void Unsubscribe(ISubscriber subscriber)
        {
            this.subscribers.Remove(subscriber);
        }

        public void Publish(string url)
        {
            foreach (ISubscriber subscriber in this.subscribers)
            {
                subscriber.OnNewVideo(this.Channel, url);
            }
        }
    }

    public class Viewer : ISubscriber
    {
        public string LastUrl { get; private set; }

        public int NotificationCount { get; private set; }

        public void OnNewVideo(string channel, string url)
        {
            this.LastUrl = url;
            this.NotificationCount = this.NotificationCount + 1;
        }
    }
}
