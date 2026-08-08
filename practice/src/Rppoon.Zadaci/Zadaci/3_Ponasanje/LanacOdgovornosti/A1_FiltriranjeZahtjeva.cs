using System;

namespace Rppoon.Zadaci.LanacOdgovornosti.A1
{
    // ================================================================
    //  LANAC ODGOVORNOSTI - razina A (vodeno), zadatak 1: Filtri zahtjeva
    // ================================================================
    //  Karika lanca (RequestFilter) je vec napisana. Tvoj posao su dva
    //  konkretna filtra.
    //
    //  Ovo je NECISTI lanac: karika i sama nesto radi, pa tek onda
    //  prosljeduje dalje. Zahtjev je valjan samo ako ga PROPUSTE SVE
    //  karike; prvi filtar koji ga odbije prekida putovanje.
    //
    //  Kljucno: filtar ne smije znati KOJI je filtar iza njega. Vidi
    //  samo 'next' tipa RequestFilter.
    // ================================================================

    /// <summary>DANO: zahtjev koji putuje lancem.</summary>
    public class Request
    {
        public Request(string user, string resource)
        {
            this.User = user;
            this.Resource = resource;
        }

        public string User { get; private set; }

        public string Resource { get; private set; }
    }

    /// <summary>DANO: provjera je li se korisnik prijavio.</summary>
    public interface IAuthenticationService
    {
        bool IsAuthenticated(string user);
    }

    /// <summary>DANO: provjera smije li korisnik do trazenog resursa.</summary>
    public interface IUserRolesService
    {
        bool HasAccess(string user, string resource);
    }

    /// <summary>DANO: obradivac - zajednicki dio svake karike lanca.</summary>
    public abstract class RequestFilter
    {
        protected RequestFilter next;

        public void SetNext(RequestFilter requestFilter)
        {
            this.next = requestFilter;
        }

        public abstract bool IsValid(Request request);
    }

    public class AuthenticationFilter : RequestFilter
    {
        private readonly IAuthenticationService authentication;

        public AuthenticationFilter(IAuthenticationService authentication)
        {
            this.authentication = authentication;
        }

        public override bool IsValid(Request request)
        {
            // TODO
            //  1. pitaj 'authentication' je li request.User prijavljen
            //  2. ako NIJE -> odmah false, lanac dalje ne ide
            //  3. ako JEST -> proslijedi sljedecoj karici
            //  4. ako sljedece karike nema, ti si zadnji - zahtjev je valjan
            throw new NotImplementedException("AuthenticationFilter.IsValid");
        }
    }

    public class AccessFilter : RequestFilter
    {
        private readonly IUserRolesService rolesService;

        public AccessFilter(IUserRolesService rolesService)
        {
            this.rolesService = rolesService;
        }

        public override bool IsValid(Request request)
        {
            // TODO: isti oblik kao gore, samo pitas 'rolesService'
            //       ima li request.User pravo na request.Resource.
            throw new NotImplementedException("AccessFilter.IsValid");
        }
    }
}
