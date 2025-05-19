using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace HospitalAppointmentSystem.Client.Auth
{
    public class DummyAuthStateProvider : AuthenticationStateProvider
    {
        private readonly Task<AuthenticationState> _authenticationState;

        public DummyAuthStateProvider()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            _authenticationState = Task.FromResult(new AuthenticationState(anonymous));
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() => _authenticationState;
    }
}