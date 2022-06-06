using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using VotingSystem.Application;
using VotingSystem.Models;

namespace VotingSystem.UI.Pages
{
    public class PollModel : PageModel
    {
        public PollStatistics Statistics { get; set; }

        public void OnGet(int id, [FromServices] StatisticsInteractor statisticsInteractor)
        {
           Statistics = statisticsInteractor.GetStatistics(id);
        }
        public IActionResult OnPost(int counterId, [FromServices] VotingInteractor votingInteractor)
        {
            var email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            votingInteractor.Vote(new Vote { CounterId = counterId, UserId = email });
            return Redirect(Request.Path.Value);
        }
    }
}
