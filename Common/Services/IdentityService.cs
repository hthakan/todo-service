using Common.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;

namespace Common.Services
{
    public class IdentityService : IIdentityService
    {
        private IHttpContextAccessor _context;
        private readonly IOptions<APISettings> _APISettings;

        public IdentityService(IHttpContextAccessor context, IOptions<APISettings> APISettings)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _APISettings = APISettings ?? throw new ArgumentNullException(nameof(APISettings));
        }

        public int GetUserIdentity()
        {
            if (_APISettings.Value.JWTEnabled)
            {

                return Convert.ToInt32(_context.HttpContext.User.FindFirst("sub").Value);
            }
            else
            {
                return 34565;
            }
        }

        public string GetUserName()
        {
            if (_APISettings.Value.JWTEnabled)
            {

                return _context.HttpContext.User.Identity.Name;
            }
            else
            {
                return "SampleName";
            }
        }
    }
}
