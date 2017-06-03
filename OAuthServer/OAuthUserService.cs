using IdentityServer3.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityServer3.Core.Models;
using System.Threading.Tasks;
using IdentityServer3.Core.Services.Default;
using OAuth.Data.Repositories;

namespace OAuthServer
{
    public class OAuthUserService : UserServiceBase
    {
        private IUserRepository userRepository;

        public OAuthUserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public override async Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            var user = await userRepository.GetAsync(context.UserName,
                HashHelper.Sha512(context.Password));
            if (user == null) {

                context.AuthenticateResult =
                    new AuthenticateResult("Incorrect credentials");
                return;
            }
            context.AuthenticateResult =
                    new AuthenticateResult(context.UserName, context.UserName);
        }
    }
}