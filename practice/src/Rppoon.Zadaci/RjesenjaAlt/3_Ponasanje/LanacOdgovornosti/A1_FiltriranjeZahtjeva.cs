namespace Rppoon.Zadaci.LanacOdgovornosti.A1
{
    // ===== ALTERNATIVNO RJESENJE - Lanac odgovornosti A1 =====
    //
    // Ugnijezdeni 'if' tocno kao u ispitnom izlistanju, zajednicko
    // prosljedivanje izvuceno u zasticenu metodu baze.

    public class Request
    {
        private readonly string user;
        private readonly string resource;

        public Request(string user, string resource)
        {
            this.user = user;
            this.resource = resource;
        }

        public string User => this.user;

        public string Resource => this.resource;
    }

    public interface IAuthenticationService
    {
        bool IsAuthenticated(string user);
    }

    public interface IUserRolesService
    {
        bool HasAccess(string user, string resource);
    }

    public abstract class RequestFilter
    {
        protected RequestFilter next;

        public void SetNext(RequestFilter requestFilter)
        {
            this.next = requestFilter;
        }

        public abstract bool IsValid(Request request);

        /// <summary>Ostatak lanca; ako ga nema, nitko vise nema primjedbi.</summary>
        protected bool OstatakLanca(Request request)
        {
            return this.next == null ? true : this.next.IsValid(request);
        }
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
            if (this.authentication.IsAuthenticated(request.User))
            {
                return this.OstatakLanca(request);
            }
            else
            {
                return false;
            }
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
            if (this.rolesService.HasAccess(request.User, request.Resource))
            {
                return this.OstatakLanca(request);
            }
            else
            {
                return false;
            }
        }
    }
}
