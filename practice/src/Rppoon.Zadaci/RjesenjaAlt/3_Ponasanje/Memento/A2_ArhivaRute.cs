using System.Collections.Generic;

namespace Rppoon.Zadaci.Memento.A2
{
    // ===== ALTERNATIVNO RJESENJE - Memento A2 =====
    //
    // Skrbnik je ovdje obicna lista iz koje se skida s kraja, umjesto
    // stoga. Isti ugovor, drugacija zbirka.
    //
    // Klijentovo ponistavanje je napisano bez privremene varijable, s
    // provjerom kroz Count. I dalje vrijedi ono glavno: snimku cita
    // ruta (Restore), a ne klijent i ne arhiva.

    public class SavedRoute
    {
        private readonly string[] waypoints;

        public SavedRoute(string destination, IEnumerable<string> waypoints)
        {
            this.Destination = destination;
            this.waypoints = new List<string>(waypoints).ToArray();
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
        private readonly List<SavedRoute> povijest = new List<SavedRoute>();

        public int Count => this.povijest.Count;

        public void Store(SavedRoute saved)
        {
            this.povijest.Add(saved);
        }

        public SavedRoute Undo()
        {
            if (this.povijest.Count == 0)
            {
                return null;
            }

            int zadnji = this.povijest.Count - 1;
            SavedRoute snimka = this.povijest[zadnji];
            this.povijest.RemoveAt(zadnji);
            return snimka;
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
            if (this.archive.Count == 0)
            {
                return false;
            }

            this.route.Restore(this.archive.Undo());
            return true;
        }
    }
}
