using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Models;

namespace Web.Api.Infrastructure.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);

        Task Register(AppUser appUser);
    }
}
