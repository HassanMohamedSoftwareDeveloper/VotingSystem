using Microsoft.AspNetCore.Mvc;
using VotingSystem.Models;

namespace VotingSystem.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVotingPollFactory _votingPollFactory;

        public HomeController(IVotingPollFactory votingPollFactory)
        {
            _votingPollFactory = votingPollFactory;
        }
        
        [HttpPost]
        public VotingPoll Create(VotingPollFactory.Request request)
        {
           return _votingPollFactory.Create(request);
        }
    }
}