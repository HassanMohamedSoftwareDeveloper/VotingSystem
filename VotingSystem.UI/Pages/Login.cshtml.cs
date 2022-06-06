using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace VotingSystem.UI.Pages
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost(string email)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,email),
            };

            var identity = new ClaimsIdentity(claims, "Voting System");

            var principle = new ClaimsPrincipal(new[] { identity });
            await HttpContext.SignInAsync(principle);

            return RedirectToPage("/Index");
        }
    }
}
