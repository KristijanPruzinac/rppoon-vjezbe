namespace Rppoon.Zadaci.LanacOdgovornosti.A1
{
    // ======= REFERENTNO RJESENJE - Lanac odgovornosti A1 =======

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
            if (!this.authentication.IsAuthenticated(request.User))
            {
                return false;
            }

            if (this.next != null)
            {
                return this.next.IsValid(request);
            }

            return true;
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
            if (!this.rolesService.HasAccess(request.User, request.Resource))
            {
                return false;
            }

            if (this.next != null)
            {
                return this.next.IsValid(request);
            }

            return true;
        }
    }
}
