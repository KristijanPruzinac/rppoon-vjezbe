using System.Collections.Generic;
using System.Linq;

namespace Rppoon.Zadaci.Promatrac.C1
{
    // ===== ALTERNATIVNO RJESENJE - Promatrac C1 =====
    //
    // Pretplatnici u HashSetu (pretplata dvaput nema ucinka), kanal u
    // izricitom polju, objava preko kopije popisa.

    public interface ISubscriber
    {
        void OnNewVideo(string channel, string url);
    }

    public class MooTuber
    {
        private readonly string channel;
        private readonly HashSet<ISubscriber> pretplatnici = new HashSet<ISubscriber>();

        public MooTuber(string channel)
        {
            this.channel = channel;
        }

        public string Channel
        {
            get { return this.channel; }
        }

        public void Subscribe(ISubscriber subscriber)
        {
            this.pretplatnici.Add(subscriber);
        }

        public void Unsubscribe(ISubscriber subscriber)
        {
            this.pretplatnici.Remove(subscriber);
        }

        public void Publish(string url)
        {
            foreach (ISubscriber pretplatnik in this.pretplatnici.ToList())
            {
                pretplatnik.OnNewVideo(this.channel, url);
            }
        }
    }

    public class Viewer : ISubscriber
    {
        private readonly List<string> primljene = new List<string>();

        public string LastUrl
        {
            get { return this.primljene.LastOrDefault(); }
        }

        public int NotificationCount
        {
            get { return this.primljene.Count; }
        }

        public void OnNewVideo(string channel, string url)
        {
            this.primljene.Add(url);
        }
    }
}
