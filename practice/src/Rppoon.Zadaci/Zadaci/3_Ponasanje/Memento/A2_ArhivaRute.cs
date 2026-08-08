using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Memento.A2
{
    // ================================================================
    //  MEMENTO - razina A (vodeno), zadatak 2: Arhiva rute
    // ================================================================
    //  Uloge su s ispita (26.6.2023., pitanje 9):
    //    NavigationRoute = tvorac, SavedRoute = memento,
    //    RouteArchive    = skrbnik.
    //
    //  A1 je bio obrnut od ovoga: ondje je skrbnik bio dan, a ti si
    //  pisao tvorca. Ovdje je tvorac dan, a ti pises SKRBNIKA i
    //  klijentovo ponistavanje.
    //
    //  Zasto bas to: najcesca pogreska kod ovog obrasca je da tvorac
    //  sam vodi svoju povijest. Kad jednom napises skrbnika, vidi se
    //  da je pamcenje zaseban posao - tvorac zna SAMO snimiti i vratiti
    //  jedno stanje, a tko cuva snimke i koliko dugo, njega ne zanima.
    //
    //  Kljucno: skrbnik nigdje ne smije citati sadrzaj snimke. Njemu je
    //  SavedRoute zatvorena kutija.
    // ================================================================

    /// <summary>DANO: memento - snimka rute.</summary>
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

    /// <summary>DANO: tvorac - ruta koja se mijenja tijekom voznje.</summary>
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

    /// <summary>SKRBNIK: cuva snimke rute i vraca ih obrnutim redom.</summary>
    public class RouteArchive
    {
        // TODO: ovdje deklariraj zbirku u kojoj cuvas snimke.
        //       Zadnja spremljena mora izaci prva.

        /// <summary>Koliko je snimki trenutno u arhivi.</summary>
        public int Count
        {
            // TODO
            get { throw new NotImplementedException("RouteArchive.Count"); }
        }

        /// <summary>Sprema snimku u arhivu.</summary>
        public void Store(SavedRoute saved)
        {
            // TODO
            throw new NotImplementedException("RouteArchive.Store");
        }

        /// <summary>
        /// Skida i vraca zadnju spremljenu snimku.
        /// Ako arhiva je prazna, vraca null (ne baca iznimku).
        /// </summary>
        public SavedRoute Undo()
        {
            // TODO
            throw new NotImplementedException("RouteArchive.Undo");
        }
    }

    /// <summary>KLIJENT: spaja rutu i arhivu.</summary>
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

        /// <summary>DANO: svaka promjena se prvo zabiljezi, pa se izvede.</summary>
        public void AddWaypoint(string waypoint)
        {
            this.archive.Store(this.route.Save());
            this.route.AddWaypoint(waypoint);
        }

        /// <summary>DANO: isto vrijedi i za promjenu odredista.</summary>
        public void ChangeDestination(string destination)
        {
            this.archive.Store(this.route.Save());
            this.route.Destination = destination;
        }

        /// <summary>
        /// Ponistava zadnju promjenu.
        /// Vraca true ako je bilo sto ponistiti, inace false.
        /// </summary>
        public bool UndoLastChange()
        {
            // TODO
            //  1. uzmi zadnju snimku iz arhive
            //  2. ako je nema -> false
            //  3. ako je ima -> reci ruti neka se vrati u to stanje, pa true
            //
            //  Primijeti tko sto radi: snimku CITA ruta, ne ti i ne arhiva.
            throw new NotImplementedException("NavigationSession.UndoLastChange");
        }
    }
}
