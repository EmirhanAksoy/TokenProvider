using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ArcelikAuthProvider.Extension
{
    public static class ControllerBaseExtension
    {
        private static string GetClaim(ControllerBase controller, string claimType)
        {
            if (controller == null)
                return string.Empty;

            var claim = controller.User.Claims.FirstOrDefault(s => s.Type == claimType);
            if (claim == null)
                return string.Empty;

            return claim.Value;
        }

        private static IList<string> GetClaims(ControllerBase controller, string claimType)
        {
            if (controller == null)
                return null;

            List<string> claims = new List<string>();

            var claimsTemp = controller.User.Claims.Where(s => s.Type == claimType);

            if (claimsTemp == null)
                return null;

            foreach (var item in claimsTemp)
            {
                claims.Add(item.Value);
            }

            return claims.ToList();
        }

        public static string GetCurrentUserId(this ControllerBase controller) =>
            GetClaim(controller, ClaimTypes.NameIdentifier);

        public static string GetCurrentUsername(this ControllerBase controller) =>
            GetClaim(controller, ClaimTypes.Name);

    }
}
