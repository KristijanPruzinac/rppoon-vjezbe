using System.Collections.Generic;

namespace Rppoon.Zadaci.Memento.A2
{
    // ===== REFERENTNO RJESENJE - Memento A2 =====
    //
    // Skrbnik je stog snimki i nista vise. Nigdje u njemu ne stoji ni
    // rijec o odredistu ni o tockama rute - da se sutra ruti doda jos
    // jedno polje, RouteArchive se ne dira.

    public class SavedRoute
    {
        private readonly List<string> waypoints;

        public SavedRoute(string destination, IEnumerable<string> waypoints)
        {
            this.Destination = destination;
            this.waypoints = new List<string>(waypoints);
        }

        public string Destination { get; private set; }

        public IReadOnlyList<string> Waypoints => this.waypoints;
    }

    public class NavigationRoute
    {
        private readonly List<string> waypoints = new List<string>();

        public NavigationRoute(string destination)
        {
            this.Destination = destination;
        }

        public string Destination { get; set; }

        public IReadOnlyList<string> Waypoints => this.waypoints;

        public void AddWaypoint(string waypoint)
        {
            this.waypoints.Add(waypoint);
        }

        public SavedRoute Save()
        {
            return new SavedRoute(this.Destination, this.waypoints);
        }

        public void Restore(SavedRoute saved)
        {
            this.Destination = saved.Destination;
            this.waypoints.Clear();
            this.waypoints.AddRange(saved.Waypoints);
        }
    }

    public class RouteArchive
    {
        private readonly Stack<SavedRoute> saved = new Stack<SavedRoute>();

        public int Count => this.saved.Count;

        public void Store(SavedRoute saved)
        {
            this.saved.Push(saved);
        }

        public SavedRoute Undo()
        {
            return this.saved.Count > 0 ? this.saved.Pop() : null;
        }
    }

    public class NavigationSession
    {
        private readonly NavigationRoute route;
        private readonly RouteArchive archive = new RouteArchive();

        public NavigationSession(NavigationRoute route)
        {
            this.route = route;
        }

        public string Destination => this.route.Destination;

        public IReadOnlyList<string> Waypoints => this.route.Waypoints;

        public void AddWaypoint(string waypoint)
        {
            this.archive.Store(this.route.Save());
            this.route.AddWaypoint(waypoint);
        }

        public void ChangeDestination(string destination)
        {
            this.archive.Store(this.route.Save());
            this.route.Destination = destination;
        }

        public bool UndoLastChange()
        {
            SavedRoute zadnja = this.archive.Undo();
            if (zadnja == null)
            {
                return false;
            }

            this.route.Restore(zadnja);
            return true;
        }
    }
}
